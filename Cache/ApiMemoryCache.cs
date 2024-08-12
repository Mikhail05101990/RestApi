using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using WebApi.Data;

namespace WebApi.Cache
{
    public class ApiMemoryCache
    {
        private long _cache = 0;
        private ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

        public async Task<long> GetOrCreate(object key)
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync();
                
            try
            {
                ++_cache;
            }
            finally
            {
                mylock.Release();
            }

            return _cache;
        }
    }
}
