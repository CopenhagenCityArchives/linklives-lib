using Elasticsearch.Net;
using Linklives.Domain;
using Nest;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;


namespace Linklives.DAL
{
    //TODO: Verify that all queries conform to the new object mapping
    public class ESPersonAppearanceRepository : IPersonAppearanceRepository
    {
        private readonly ElasticClient client;
        private readonly ISourceRepository sourceRepository;

        public ESPersonAppearanceRepository(ElasticClient client, ISourceRepository sourceRepository)
        {
            this.client = client;
            this.sourceRepository = sourceRepository;
        }

        public BasePA GetById(string id)
        {
            var searchResponse = client.Get<BasePA>(id, g => g.Index("pas"));
            searchResponse.Source.Source = sourceRepository.GetById(searchResponse.Source.Source_id);
            return searchResponse.Source;
        }

        public IEnumerable<BasePA> GetByIds(List<string> Ids)
        {
            //TODO: Fix this up and tag on the transcribed source before returning
            var pasSearchResponse = client.Search<BasePA>(s => s
            .Index("pas")
            .From(0)
            .Size(1000)
            .Query(q => q
                .Terms(t => t
                    .Field("key")
                    .Terms(Ids))));
            //var pass = pasSearchResponse.Documents.Select(x => x["person_appearance"]).ToList();
            //var sourceids = pass.Select(p => (int)p["source_id"]).ToList();
            //var sources = sourceRepository.GetByIds(sourceids);
            //foreach (var pas in pass)
            //{
            //    pas.Add("source", sources.Single(s => (int)s["source_id"] == (int)pas["source_id"]));
            //}
            return pasSearchResponse.Documents;
        }
    }
}
