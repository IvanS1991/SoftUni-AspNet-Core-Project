using Microsoft.Extensions.Caching.Memory;

namespace PropertyAds.WebApp.Services.Utility
{
    public class Cache : ICache
    {
        private readonly IMemoryCache cache;

        public Cache(
            IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void Remove(object key)
        {
            this.cache.Remove(key);
        }

        public T Set<T>(object key, T value)
        {
            return this.cache.Set(key, value);
        }

        public bool TryGetValue<T>(object key, out T value)
        {
            return this.cache.TryGetValue(key, out value);
        }
    }
}
