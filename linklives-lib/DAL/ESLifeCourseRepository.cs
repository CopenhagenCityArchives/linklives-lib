using Linklives.Domain;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.DAL
{
    public class ESLifeCourseRepository : IKeyedRepository<LifeCourse>
    {
        private readonly ElasticClient client;

        public ESLifeCourseRepository(ElasticClient client)
        {
            this.client = client;
        }

        public LifeCourse GetByKey(string key)
        {
            var searchResponse = client.Get<LifeCourse>(key, g => g.Index("lifecourses"));

            return searchResponse.Source;
        }

        public IEnumerable<LifeCourse> GetByKeys(IList<string> keys)
        {
            var searchResponse = client.Search<LifeCourse>(s => s
            .Index("lifecourses")
            .From(0)
            .Size(1000)
            .Query(q => q
                .Terms(t => t
                    .Field("lifecourse.key")
                    .Terms(keys))));

            return searchResponse.Documents;
        }
    }
}
