using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Abstraction.Messaging
{
    public interface ICacheServices
    {
        public Task<T> GetCacheAsync<T>(string cacheKey, CancellationToken token = default);
        public Task SetCacheAsync<T>(string cacheKey, T value, TimeSpan? slidingExpiration = null, CancellationToken token = default);
        public Task RemoveCachePatternAsync(string cachePattern, CancellationToken token = default);
        public Task RemoveCacheAsync(string cacheKey, CancellationToken token = default);
    }
}
