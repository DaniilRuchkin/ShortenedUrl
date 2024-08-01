using URLShortener.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed; 
using Newtonsoft.Json;

namespace URLShortener.Infrastructure.Services;

public class RedisCacheService(IDistributedCache cache) : IRedisCacheService
{
    public async Task<T> GetCachedDataAsync<T>(string key)
    {
        var cacheData = await cache.GetStringAsync(key);
        
        if (cacheData != null )
        {
            return JsonConvert.DeserializeObject<T>(cacheData)!;
        }
        
        return default!;
    }

    public async Task SetCachedDataAsync<T>(string key, T data, int deleteTimeInHours)
    {
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(deleteTimeInHours)
        };

        await cache.SetStringAsync(key, JsonConvert.SerializeObject(data), cacheOptions);
    }

    public async Task RemoveCachedData(string key)
    {
        await cache.RemoveAsync(key);
    }
}