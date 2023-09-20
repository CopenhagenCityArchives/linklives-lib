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

            var pas = client.MultiGet(m => m.Index("pas").GetMany<BasePA>(ids))
                .GetMany<BasePA>(ids)
                .Select((hit) => hit.Source)
                .Where((pa) => {
                    if(pa == null || pa.Source == null) {
                        var sourceString = pa.Source == null ? "(no source)" : pa.Source.ToString();
                        System.Console.WriteLine($"Found pa null or without source id {pa} {sourceString}");
                        return false;
                    }
                    return true;
                })
                .ToList();

            var transcribedPasByPaId = transcribedRepository.GetByIds(ids)
                .ToDictionary(t => t.Key, t => t);

            var sourceIds = new HashSet<int>();
            foreach(var pa in pas) {
                sourceIds.Add(pa.Source.Source_id);
            }

            var sourcesBySourceId = sourceRepository.GetByIds(sourceIds.ToList())
                .ToDictionary(s => s.Source_id, s => s);

            return pas
                .Where((pa) => {
                    if(!sourcesBySourceId.ContainsKey(pa.Source.Source_id)) {
                        System.Console.WriteLine($"No source found for PA source with source_id {pa.Source.Source_id}");
                        return false;
                    }
                    return true;
                })
                .Select((pa) => BasePA.Create(
                    sourcesBySourceId[pa.Source.Source_id],
                    pa.Standard,
                    transcribedPasByPaId[pa.Key]
                ));
        }
    }
}
