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

            //TODO: This can be more effective (right now each pa requires 3 ES requests)
            var pas = new List<BasePA>();
            foreach (var id in Ids)
            {
                pas.Add(GetById(id));
            }

            return pas;
        }
    }
}
