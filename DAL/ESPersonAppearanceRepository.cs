using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;


namespace Linklives.DAL
{
    public class ESPersonAppearanceRepository : IPersonAppearanceRepository
    {
        private readonly ElasticClient client;
        private readonly ISourceRepository sourceRepository;

        public ESPersonAppearanceRepository(ElasticClient client, ISourceRepository sourceRepository)
        {
            this.client = client;
            this.sourceRepository = sourceRepository;
        }

        public dynamic GetById(string id)
        {
            var searchResponse = client.Get<dynamic>(id, g => g.Index("pas"));
            dynamic pas = searchResponse.Source["person_appearance"];
            pas.Add("source", sourceRepository.GetById((int)pas["source_id"]));

            return pas;
        }

        public IEnumerable<dynamic> GetByIds(List<string> Ids)
        {
            var pasSearchResponse = client.Search<dynamic>(s => s
            .Index("pas")
            .From(0)
            .Size(1000)
            .Query(q => q
                    .Nested(n => n
                    .Path("person_appearance")
                    .Query(nq => nq
                        .Terms(t => t
                            .Field("person_appearance.id")
                            .Terms(Ids))))));
            var pass = pasSearchResponse.Documents.Select(x => x["person_appearance"]).ToList();
            var sourceids = pass.Select(p => (int)p["source_id"]).ToList();
            var sources = sourceRepository.GetByIds(sourceids);
            foreach (var pas in pass)
            {
                pas.Add("source", sources.Single(s => (int)s["source_id"] == (int)pas["source_id"]));
            }
            return pass;
        }
    }
}
