using Linklives.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.DAL
{
    public class EFLifeCourseRepository : EFKeyedRepository<LifeCourse>, IEFLifeCourseRepository
    {
        private readonly DbContextOptions<LinklivesContext> contextOptions;
        private readonly int batchSize = 1000;
        public EFLifeCourseRepository(LinklivesContext context, DbContextOptions<LinklivesContext> options) : base(context)
        {
            contextOptions = options;
        }

        public IEnumerable<string> GetKeysByUserId(string userId)
        {
            var userLinkRatings = context.LinkRatings
               .Where(lr => lr.User.Equals(userId))
               .AsNoTracking()
               .Select(lr => lr.LinkId)
               .ToList();

            var links = context.Links.Where(l => userLinkRatings.Contains(l.Id)).AsNoTracking().Select(l => l).ToList();
            var lifecourseskeys = context.Links.AsNoTracking().Where(l => userLinkRatings.Contains(l.Id)).SelectMany(l => l.LifeCourses).Select(lc => lc.Key).ToList();

            return lifecourseskeys;
        }

        public LifeCourse GetByLinkId(int linkId)
        {
            Link link = null;
            try
            {
               link = context.Links.Where(l => l.Id == linkId).First();
            }
            catch(Exception e){
                throw new InvalidOperationException($"No link with id {linkId} found.");
            }

            return link.LifeCourses.First();

        }

        public void GetLinksAndRatings(LifeCourse lc)
        {
            if (lc.Links != null)
            {
                return;
            }
            lc.Links = context.Links.Where(link => link.LifeCourses.Contains(lc)).Include(link => link.Ratings).ThenInclude(rating => rating.Rating).ToList();
        }

        private void ResetContext()
        {
            context = null;
            context = new LinklivesContext(contextOptions);
        }

        private void Flush(int inserts, bool force)
        {
            if (inserts % batchSize == 0 || force == true)
            {
                try
                {
                    System.Console.WriteLine($"Upserting, {inserts} inserts");
                    context.SaveChanges();
                    ResetContext();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = entry.GetDatabaseValues();
                    var databaseValuesAsBlog = (Link)databaseValues.ToObject();
                    System.Console.WriteLine(ex.Message);
                }
                catch(DbUpdateException e)
                {
                    System.Console.WriteLine();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Could not insert data");
                }
            }
        }
        /// Upserts LifeCourses and Links
        /// Lifecourses that already exists is updated with the given dataVersion.
        /// Note that this method is often used in conjunction with
        /// MarkOldItems, whichs updates old items in the database to Is_historic
        /// Note that it is assumed that the keys in items are unique!
        /// </summary>
        /// <param name="items">Items that reflect the current state of data</param>
        /// <param name="dataVersion">The new DataVersion</param>
        public void Upsert(IEnumerable<LifeCourse> items, string dataVersion)
        {
            //context.ChangeTracker.AutoDetectChangesEnabled = false;
            var existingLifeCourses = context.LifeCourses.AsNoTracking().AsEnumerable().Select(lc => new { Key = lc.Key, Id = lc.Id }).ToDictionary(keyId => keyId.Key, keyId => keyId.Id);
            var existingLinks = context.Links.AsNoTracking().AsEnumerable().Select(lc => new { Key = lc.Key, Id = lc.Id }).ToDictionary(keyId => keyId.Key, keyId => keyId.Id);
            
            int inserts = 0;
            foreach(var lc in items)
            {
                lc.Data_version = dataVersion;

                if (existingLifeCourses.ContainsKey(lc.Key))
                {
                    lc.Id = existingLifeCourses[lc.Key];
                }
                foreach(var link in lc.Links)
                {
                    if (existingLinks.ContainsKey(link.Key))
                    {
                        link.Id = existingLinks[link.Key];
                    }
                }
                context.Attach(lc);
                inserts++;
                Flush(inserts, false);
            }

            Flush(0, true);
        }

        /// <summary>
        /// Updates items in DB that does not exists in the new items list with is_historic = true
        /// All other items are ignored.
        /// </summary>
        /// <param name="newItems">Items that reflect the current state of data</param>
        public void MarkOldItems<U>(IEnumerable<U> newItems) where U : KeyedItem
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            var newItemIDs = newItems.Select(lc => lc.Key).ToDictionary(x => x, x => true);

            // Get list of life courses in the DB that is not present in the list
            var itemsInDbNotInNewItems = context.Set<U>().AsEnumerable().Where(lc => !newItemIDs.ContainsKey(lc.Key));

            // Update old items to be historic
            foreach (U lc in itemsInDbNotInNewItems)
            {
                // Dont save related entities, these must be added individually
                context.Entry(lc).State = EntityState.Detached;

                // These items are historic
                lc.Is_historic = true;

                context.Set<U>().Attach(lc);
            }

            context.SaveChanges();
        }
    }
}
