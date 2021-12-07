using Linklives.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.DAL
{
    public class EFLifeCourseRepository : EFKeyedRepository<LifeCourse>, ILifeCourseRepository
    {
        public EFLifeCourseRepository(LinklivesContext context) : base(context)
        {
        }

        public IEnumerable<LifeCourse> GetByUserRatings(string userId)
        {
            var lifecourseskeys = context.LinkRatings.Where(lr => lr.User == userId).Include(x => x.Link.LifeCourses).SelectMany(lr => lr.Link.LifeCourses.Select(x => x.Key)).Distinct().ToList();
            return GetByKeys(lifecourseskeys);
        }

        public void GetLinks(LifeCourse lc)
        {
            if (lc.Links != null)
            {
                return;
            }

            context.Entry(lc).Collection(b => b.Links).Load();
        }

        /// Adds items to the DB that are not already present.
        /// All other items are ignored.
        /// </summary>
        /// <param name="newItems">Items that reflect the current state of data</param>
        /// <param name="dataVersion">The new DataVersion</param>
        public void AddNewItems(IEnumerable<LifeCourse> newItems, string dataVersion)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            var IDsInTheDatabase = context.Set<LifeCourse>().AsNoTracking().AsEnumerable().Select(lc => lc.Key).ToDictionary(x => x, x => true);

            // items not in the database
            var itemsNotInDb = newItems.Where(lc => !IDsInTheDatabase.ContainsKey(lc.Key));

            /*var existingLinks = context.Set<Link>().AsNoTracking().AsEnumerable().Select(link => link.Key).ToHashSet();
            var linksInNewItems = new HashSet<string>();

            // Get existing links in new LifeCourses
            foreach (var lc in itemsNotInDb)
            {
                foreach(var link in lc.Links)
                {
                    linksInNewItems.Add(link.Key);
                }
            }

            // Get id of links in new items which does not exist
            var linkIdsToAdd = linksInNewItems.Except(existingLinks);*/

            // Add items not in the database
            foreach (LifeCourse lc in itemsNotInDb)
            {
                // These items are not historic
                lc.Is_historic = false;

                // Set the data version to the newest
                lc.Data_version = dataVersion;

                // Only add links that are needed
                //lc.Links = lc.Links.Where(link => linkIdsToAdd.Contains(link.Key)).ToList();

                context.Set<LifeCourse>().Add(lc);
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Updates items in DB that does not exists in the new items list with is_historic = true
        /// All other items are ignored.
        /// </summary>
        /// <param name="newItems">Items that reflect the current state of data</param>
        public void MarkOldItems(IEnumerable<LifeCourse> newItems)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            var newItemIDs = newItems.Select(lc => lc.Key).ToDictionary(x => x, x => true);

            // Get list of life courses in the DB that is not present in the list
            var itemsInDbNotInNewItems = context.Set<LifeCourse>().AsEnumerable().Where(lc => !newItemIDs.ContainsKey(lc.Key));

            // Update old items to be historic
            foreach (LifeCourse lc in itemsInDbNotInNewItems)
            {
                // Dont save related entities, these must be added individually
                context.Entry(lc).State = EntityState.Detached;

                // These items are historic
                lc.Is_historic = true;

                context.Set<LifeCourse>().Attach(lc);
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Updates existing items in DB with a new DataVersion based on the upsertItems list
        /// All other items are ignored.
        /// </summary>
        /// <param name="newItems">Items that reflect the current state of data</param>
        /// <param name="dataVersion">The new DataVersion</param>
        public void UpdateExistingItems(IEnumerable<LifeCourse> newItems, string dataVersion)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            var IDsInTheDatabase = context.Set<LifeCourse>().AsNoTracking().AsEnumerable().Select(lc => lc.Key).ToDictionary(x => x, x => true);

            // Select ids from items which are in the database
            var itemsAlreadyInDb = newItems.Where(lc => IDsInTheDatabase.ContainsKey(lc.Key) && !lc.Data_version.Equals(dataVersion));

            // Update items already in the database
            foreach (LifeCourse lc in itemsAlreadyInDb)
            {
                // These items are not historic
                lc.Is_historic = false;

                // Set the data version to the newest
                lc.Data_version = dataVersion;

                // Dont save related entities, these must be updated individually
                context.Entry(lc).State = EntityState.Detached;

                context.Set<LifeCourse>().Attach(lc);
            }

            context.SaveChanges();
        }
    }
}
