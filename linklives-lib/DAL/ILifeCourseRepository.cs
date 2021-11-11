using Linklives.Domain;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface ILifeCourseRepository
    {
        int Count();
        IEnumerable<LifeCourse> GetAll();
        LifeCourse GetByKey(string lifeCourseKey);
        IEnumerable<LifeCourse> GetByKeys(IList<string> lifecourseskeys);
        void GetLinks(LifeCourse lc);
        IEnumerable<LifeCourse> GetByUserRatings(string userId);
        void Insert(LifeCourse lifeCourse);
        void Insert(IEnumerable<LifeCourse> lifeCourses);
        void Upsert(IEnumerable<LifeCourse> entitties);
        void Delete(string lifeCourseKey);
        void Save();
    }
}
