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

        public void UpsertLifeCoursesMarkOldOnes(IEnumerable<LifeCourse> lifecourses)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            var IDsInTheDatabase = context.LifeCourses.AsEnumerable().Select(lc => lc.Key).ToDictionary(x => x, x => true);

            // Select ids from lifecourses which are in the database
            var lifecoursesAlreadyInDb = lifecourses.Where(lc => IDsInTheDatabase.ContainsKey(lc.Key));

            // Update lifecourses already in the database
            foreach (LifeCourse lc in lifecoursesAlreadyInDb)
            {
                //These lifecourses are not historic
                lc.Is_historic = false;
                context.Update(lc);
            }

            context.SaveChanges();

            // Lifecourses not in the database
            var lifeCoursesNotInDb = lifecourses.Where(lc => !IDsInTheDatabase.ContainsKey(lc.Key));

            // Add lifecourses not in the database
            foreach (LifeCourse lc in lifeCoursesNotInDb)
            {
                lc.Links = null;
                lc.Is_historic = false;
                context.Add(lc);
            }

            context.SaveChanges();

            var newLifeCourseIDs = lifecourses.Select(lc => lc.Key).ToDictionary(x => x, x => true);

            // Get list of life courses in the DB that is not present in the list
            var lifecoursesInDbNotInNewLifeCourses = context.LifeCourses.Where(lc => !newLifeCourseIDs.ContainsKey(lc.Key)).ToArray();

            // Update old lifecourses to be historic
            foreach (LifeCourse lc in lifecoursesInDbNotInNewLifeCourses)
            {
                lc.Links = null;
                lc.Is_historic = true;
                context.Add(lc);
            }

            context.SaveChanges();
        }
    }
}
