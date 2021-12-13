using Linklives.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.DAL
{
    public class EFLifeCourseRepository : EFKeyedRepository<LifeCourse>, ILifeCourseRepository
    {
        private readonly DbContextOptions<LinklivesContext> contextOptions;
        public EFLifeCourseRepository(LinklivesContext context, DbContextOptions<LinklivesContext> options) : base(context)
        {
            contextOptions = options;
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

        public void ResetContext()
        {
            context = null;
            context = new LinklivesContext(contextOptions);
        }

        /// Adds items to the DB that are not already present.
        /// All other items are ignored.
        /// </summary>
        /// <param name="newItems">Items that reflect the current state of data</param>
        /// <param name="dataVersion">The new DataVersion</param>
        public void AddNewItems(IEnumerable<LifeCourse> newItems, string dataVersion)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            var IDsInTheDatabase = context.Set<LifeCourse>().AsNoTracking().AsEnumerable().Select(lc => lc.Key).ToHashSet();

            // items not in the database
            var itemsNotInDb = newItems.Where(lc => !IDsInTheDatabase.Contains(lc.Key));

            var existingLinks = context.Set<Link>().AsNoTracking().AsEnumerable().Select(link => link.Key).ToHashSet();

            // Add items not in the database
            int inserts = 0;
            var linksToAdd = new List<Link>();
            foreach (LifeCourse lc in itemsNotInDb)
            {
                // These items are not historic
                lc.Is_historic = false;

                // Set the data version to the newest
                lc.Data_version = dataVersion;

                foreach (Link link in lc.Links)
                {
                    if (!existingLinks.Contains(link.Key))
                    {
                        context.Links.Add(link);
                        existingLinks.Add(link.Key);
                    }
                }

                if (!IDsInTheDatabase.Contains(lc.Key))
                {
                    context.LifeCourses.Add(lc);
                }

                inserts++;

                if (inserts % 1000 == 0)
                {
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e.Message);
                    }
                    ResetContext();
                }
            }

            ResetContext();
        }

        public void InsertNewLifeCourseLinks(IEnumerable<LifeCourse> lcs)
        {
            int inserts = 0;
            foreach (LifeCourse lc in lcs)
            {
                foreach (Link l in lc.Links)
                {
                    //        context.LifeCourseLink.Add(new LifeCourseLink() { LifeCoursesKey = lc.Key, LinksKey = l.Key });
                }

                inserts++;

                if (inserts % 1000 == 0)
                {
                    context.SaveChanges();
                    ResetContext();
                }
            }

            context.SaveChanges();
        }

        public void UpsertKeyedItems<U>(IEnumerable<U> items, string dataVersion) where U : KeyedItem
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            var existingItems = context.Set<U>().AsNoTracking().AsEnumerable().Select(item => item.Key).ToHashSet();
            var itemsToAdd = new List<U>();
            var itemsToUpsert = new List<U>();
            foreach (U l in items)
            {
                if (!existingItems.Contains(l.Key))
                {
                    itemsToAdd.Add(l);
                }
                else
                {
                    l.Is_historic = false;
                    l.Data_version = dataVersion;
                    itemsToUpsert.Add(l);
                }
            }

            context.AddRange(itemsToAdd);
            context.SaveChanges();
            ResetContext();

            context.AddRange(itemsToUpsert);
            context.SaveChanges();
            ResetContext();
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
