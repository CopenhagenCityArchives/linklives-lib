using Linklives.Domain;
using System.Collections.Generic;

namespace Linklives.DAL
{
    public interface IEFLifeCourseRepository : IKeyedRepository<LifeCourse>
    {
        int Count();
        IEnumerable<LifeCourse> GetAll();

        LifeCourse GetByLinkId(int linkId);

        void GetLinksAndRatings(LifeCourse lc);
        IEnumerable<string> GetKeysByUserId(string userId);
        void Insert(LifeCourse lifeCourse);
        void Insert(IEnumerable<LifeCourse> lifeCourses);
        void Upsert(IEnumerable<LifeCourse> entitties);
        void Delete(string lifeCourseKey);
        void Save();
    }
}
