using Linklives.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.DAL
{
    public abstract class EFKeyedRepository<T> : DBRepository<T> where T : KeyedItem
    {
        protected EFKeyedRepository(LinklivesContext context) : base(context)
        {
        }

        public void Delete(string key)
        {
            var entity = context.Set<T>().Find(key);
            context.Set<T>().Remove(entity);
        }
        public T GetByKey(string key)
        {
            return context.Set<T>().IncludeAll().SingleOrDefault(x => x.Key == key);
        }
        public IEnumerable<T> GetByKeys(IList<string> keys)
        {
            return context.Set<T>().IncludeAll().Where(x => keys.Contains(x.Key));
        }
        public void Insert(IEnumerable<T> entitties)
        {
            var newEntryKeys = entitties.Select(x => x.Key).Distinct().ToArray();
            var keysExiststingInDb = context.Set<T>().Where(x => newEntryKeys.Contains(x.Key)).Select(x => x.Key).ToArray();
            var newEntities = entitties.Where(x => !keysExiststingInDb.Contains(x.Key));

            context.Set<T>().AddRange(newEntities);
        }
        public void Upsert(IEnumerable<T> entitties)
        {
            context.Set<T>().BulkMerge(entitties, options =>
            {
                options.IncludeGraph = true;
            });
        }
        /// <summary>
        /// Upserts KeyedItems and marking old ones by:
        /// * Updating existing items (Is_historic = false and setting the Data_version)
        /// * Inserting new items (Is_historic = false and with the updated Data_version)
        /// * Updating db items not in new items (Is_historic = false and with unchanged Data_version)
        /// Note that related entities are not updated
        /// </summary>
        /// <typeparam name="U">A KeyedItem</typeparam>
        /// <param name="upsertItems">A IEnumerable\<KeyedItem\> with items to updated</param>
        /// <param name="dataVersion">A string consisting of the new data version</param>
        public void InsertItemsUpdateExistingItems<U>(IEnumerable<U> upsertItems, string dataVersion) where U : KeyedItem
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            
            var IDsInTheDatabase = context.Set<U>().AsNoTracking().AsEnumerable().Select(lc => lc.Key).ToDictionary(x => x, x => true);

            // Select ids from items which are in the database
            var itemsAlreadyInDb = upsertItems.Where(lc => IDsInTheDatabase.ContainsKey(lc.Key) && !lc.Data_version.Equals(dataVersion));

            // Update items already in the database
            foreach (U lc in itemsAlreadyInDb)
            {
                // These items are not historic
                lc.Is_historic = false;

                // Set the data version to the newest
                lc.Data_version = dataVersion;

                // Dont save related entities, these must be updated individually
                context.Entry(lc).State = EntityState.Detached;

                context.Set<U>().Attach(lc);
            }

            context.SaveChanges();

            // items not in the database
            var itemsNotInDb = upsertItems.Where(lc => !IDsInTheDatabase.ContainsKey(lc.Key));

            // Add items not in the database
            foreach (U lc in itemsNotInDb)
            {
                // Dont save related entities, these must be added individually
                context.Entry(lc).State = EntityState.Detached;
                
                // These items are not historic
                lc.Is_historic = false;

                // Set the data version to the newest
                lc.Data_version = dataVersion;

                context.Set<U>().Add(lc);
            }

            context.SaveChanges();

            var upsertItemIDs = upsertItems.Select(lc => lc.Key).ToDictionary(x => x, x => true);

            // Get list of life courses in the DB that is not present in the list
            var itemsInDbNotInNewItems = context.Set<U>().AsEnumerable().Where(lc => !upsertItemIDs.ContainsKey(lc.Key));

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
