using Linklives.Domain;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.DAL
{
    //TODO: Verify that all queries conform to the new object mapping
    public class ESSourceRepository : ISourceRepository
    {
        private readonly ElasticClient client;

        public ESSourceRepository(ElasticClient client)
        {
            this.client = client;
        }

        public IEnumerable<dynamic> GetAll()
        {
            var searchResponse = client.Search<dynamic>(s => s
            .Size(1000)
            .Index("sources")
            .Query(q => q.MatchAll()));
            return searchResponse.Documents.Select(x => x["source"]);
        }

        public Source GetById(int id)
        {
            var searchResponse = client.Get<Source>(id, g => g.Index("source"));

            return searchResponse.Source;
        }

        public IEnumerable<dynamic> GetByIds(IList<int> ids)
        {
            var searchResponse = client.Search<dynamic>(s => s
            .Index("sources")
            .From(0)
            .Size(1000)
            .Query(q => q
                    .Nested(n => n
                    .Path("source")
                    .Query(nq => nq
                        .Terms(t => t
                            .Field("source.source_id")
                            .Terms(ids))))));
            return searchResponse.Documents.Select(s => s["source"]);
        }
    }
}
