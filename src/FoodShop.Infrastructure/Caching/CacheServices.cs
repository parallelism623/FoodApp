using FoodShop.Application.Common.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System.Text;

namespace FoodShop.Infrastructure.Caching
{
    public class CacheServices : ICacheServices
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplerxer;    
        public CacheServices(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplerxer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplerxer = connectionMultiplerxer;
        }

        private byte[] Serialize<T>(T item) =>
            Encoding.Default.GetBytes(JsonConvert.SerializeObject(item, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

        private T Deserialize<T>(byte[] cachedData) =>
            JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(cachedData));

        public async Task<T> GetCacheAsync<T>(string cacheKey, CancellationToken token = default)
            => await GetAsync(cacheKey, token) is { } data
               ? Deserialize<T>(data) : default;
        private async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
        {
            return await _distributedCache.GetAsync(key, token) ?? null;
        }
        public Task RemoveCachePatternAsync(string cachePattern, CancellationToken token = default)
            => RemoveCacheByPatternAsync(cachePattern, token);

        public async Task SetCacheAsync<T>(string cacheKey, T value, TimeSpan? slidingExpiration = null, CancellationToken token = default)
            => await SetCacheAsync<T>(cacheKey, Serialize<T>(value), slidingExpiration, token);
        private async Task SetCacheAsync<T>(string cacheKey, byte[] data, TimeSpan? slidingExpiration = null,  CancellationToken token = default)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration
            };
            await _distributedCache.SetAsync(cacheKey, data, options, token);
        }
        private async Task RemoveCacheByPatternAsync(string pattern, CancellationToken token = default)
        {
            var endpoints = _connectionMultiplerxer.GetEndPoints();
            foreach(var end in endpoints)
            {
                var server = _connectionMultiplerxer.GetServer(end);
                var keys = server.Keys(pattern: pattern);
                foreach(var key in keys)
                {
                    await _distributedCache.RemoveAsync(key, token);
                }
            }
        }

        public async Task RemoveCacheAsync(string cacheKey, CancellationToken token = default)
            => await _distributedCache.RemoveAsync(cacheKey, token);
    }
}
