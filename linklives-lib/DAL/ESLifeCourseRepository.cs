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
            if(keys.Count == 0) { return new List<LifeCourse>(); }

            var searchResponse = client.Search<LifeCourse>(s => s
            .Index("lifecourses")
            .From(0)
            .Size(100)
            .Query(q => q
                .Terms(t => t
                    .Field("key")
                    .Terms(keys))));

            return searchResponse.Documents;
        }
    }
}
