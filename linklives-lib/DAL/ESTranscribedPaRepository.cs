using Linklives.Domain;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public TranscribedPA GetById(string id)
        {
            var searchResponse = client.Get<TranscribedPA>(id, g => g.Index("transcribed"));
            return searchResponse.Source;
        }

        public IEnumerable<TranscribedPA> GetByIds(List<string> ids) {
            return client.MultiGet(m => m.Index("transcribed").GetMany<TranscribedPA>(ids))
                .GetMany<TranscribedPA>(ids)
                .Select((hit) => hit.Source)
                .Where((hit) => hit != null);
        }

        public IList<TranscribedPA> GetBySource(int sourceId)
        {
            //TODO: It easily takes a couple of minutes to scroll through all results, can we improve on this?
            //TODO: Make the timeout configurable or take it in as a parameter
            string scrollTimeout = "2m";
            var initialResponse = client.Search<TranscribedPA>(s => s
                .Index("transcribed")
                .From(0)
                .Size(10000)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Source_id)
                        .Query(sourceId.ToString())))
                .Scroll(scrollTimeout));

            var results = new List<TranscribedPA>();

            if (!initialResponse.IsValid || string.IsNullOrEmpty(initialResponse.ScrollId))
                throw new Exception(initialResponse.ServerError.Error.Reason);

            if (initialResponse.Documents.Any())
                results.AddRange(initialResponse.Documents);

            string scrollid = initialResponse.ScrollId;
            bool scrollSetHasData = true;
            while (scrollSetHasData)
            {
                ISearchResponse<TranscribedPA> loopingResponse = client.Scroll<TranscribedPA>(scrollTimeout, scrollid);
                if (loopingResponse.IsValid)
                {
                    results.AddRange(loopingResponse.Documents);
                    scrollid = loopingResponse.ScrollId;
                }
                scrollSetHasData = loopingResponse.Documents.Any();
            }

            client.ClearScroll(new ClearScrollRequest(scrollid));
            return results;
        }
    }
}
