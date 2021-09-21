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
            var lifecourseskeys = context.LinkRatings.Where(lr => lr.User == userId).Include(x => x.Link.LifeCourse).Select(lr => lr.Link.LifeCourse.Key).Distinct().ToList();
            return GetByKeys(lifecourseskeys);
        }
    }
}
