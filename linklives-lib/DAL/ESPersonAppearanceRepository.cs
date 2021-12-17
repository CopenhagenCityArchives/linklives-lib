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
        private readonly ITranscribedPARepository transcribedRepository;

        public ESPersonAppearanceRepository(ElasticClient client, ISourceRepository sourceRepository, ITranscribedPARepository transcribedPARepository)
        {
            this.client = client;
            this.sourceRepository = sourceRepository;
            this.transcribedRepository = transcribedPARepository;
        }

        public BasePA GetById(string id)
        {
            var basePADoc = client.Get<BasePA>(id, g => g.Index("pas"));
            var source = sourceRepository.GetById(basePADoc.Source.Source_id);
            var transcribedPA = transcribedRepository.GetById(id);
            var pa = BasePA.Create(source, basePADoc.Source.Standard, transcribedPA);
            return pa;
        }

        public IEnumerable<BasePA> GetByIds(List<string> Ids)
        {
            if (Ids.Count == 0)
            {
                return null;
            }
            //TODO: Fix this up and tag on the transcribed pa before returning
            var pasSearchResponse = client.Search<BasePA>(s => s
                .Index("pas")
                .Query(q => q
                    .Ids(i => i
                        .Values(Ids)
                    )
                )
            );
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
