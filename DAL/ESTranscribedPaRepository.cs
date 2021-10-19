using Linklives.Domain;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.DAL
{
    public class ESTranscribedPaRepository : ITranscribedPARepository
    {
        private readonly ElasticClient client;

        public ESTranscribedPaRepository(ElasticClient client)
        {
            this.client = client;
        }

        public IEnumerable<TranscribedPA> GetBySource(int sourceId)
        {
            var searchResponse = client.Search<TranscribedPA>(s => s
                .Index("Transcribed")
                .From(0)
                .Size(int.MaxValue)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Source_id)
                        .Query(sourceId.ToString()))));
            return searchResponse.Documents;

        }
    }
}
