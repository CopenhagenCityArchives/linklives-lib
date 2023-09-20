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
            if (!basePADoc.Found)
            {
                return null;
            }
            var source = sourceRepository.GetById(basePADoc.Source.Source_id);
            var transcribedPA = transcribedRepository.GetById(id);
            var pa = BasePA.Create(source, basePADoc.Source.Standard, transcribedPA);
            return pa;
        }

        public IEnumerable<BasePA> GetByIds(List<string> ids)
        {
            if (ids.Count == 0)
            {
                return new List<BasePA>();
            }

            var pas = client.MultiGet((m) => {
                return m.GetMany<BasePA>(ids, (operation, id) => operation.Index("pas"));
            })
                .GetMany<BasePA>(ids)
                .Select((hit) => hit.Source)
                .ToList();

            var transcribedPasByPaId = transcribedRepository.GetByIds(ids)
                .ToDictionary(t => t.Pa_id, t => t);

            var sourceIds = new HashSet<int>();
            foreach(var pa in pas) {
                sourceIds.Add(pa.Source.Source_id);
            }

            var sourcesBySourceId = sourceRepository.GetByIds(sourceIds.ToList())
                .ToDictionary(s => s.Source_id, s => s);

            return pas
                .Select((pa) => BasePA.Create(
                    sourcesBySourceId[pa.Source.Source_id],
                    pa.Standard,
                    transcribedPasByPaId[pa.Pa_id]
                ));
        }
    }
}
