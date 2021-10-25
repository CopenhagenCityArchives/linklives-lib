﻿using Nest;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.DAL
{
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

        public dynamic GetById(int id)
        {
            var searchResponse = client.Search<dynamic>(s => s
            .Index("sources")
            .From(0)
            .Size(1)
            .Query(q => q
                    .Nested(n => n
                    .Path("source")
                    .Query(nq => nq
                        .Terms(t => t
                            .Field("source.source_id")
                            .Terms(id))))));
            return searchResponse.Documents.Single()["source"];
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