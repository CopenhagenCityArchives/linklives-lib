using Linklives.Domain;
using Microsoft.Extensions.Caching.Memory;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.DAL
{
    //TODO: Verify that all queries conform to the new object mapping
    public class ESSourceRepository : ISourceRepository
    {
        private readonly ElasticClient _client;
        private IMemoryCache _cache;
        private const string _sourcesListCacheKey = "sourcesList";
        private MemoryCacheEntryOptions _cacheOptions;

        public ESSourceRepository(ElasticClient client, IMemoryCache cache)
        {
            _client = client;

            _cache = cache ?? throw new ArgumentNullException(nameof(cache));

            // Set cache duration to maximum 10 hours
            _cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3600))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(36000))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(100);
        }

        public IEnumerable<Source> GetAll()
        {
            if (!_cache.TryGetValue(_sourcesListCacheKey, out List<Source> sources))
            {
                var searchResponse = _client.Search<Source>(s => s
                    .Size(1000)
                    .Index("sources")
                    .Query(q => q.MatchAll()));

                sources = searchResponse.Documents.ToList();
                _cache.Set(_sourcesListCacheKey, sources);
            }

            return sources;
        }

        public Source GetById(int id)
        {
            return GetAll().Where(s => s.Source_id == id).First();
        }

        public IEnumerable<Source> GetByIds(IList<int> ids)
        {
            return GetAll().Where(s => ids.Contains(s.Source_id));
        }
    }
}
