using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PersonalBlog.Application.Caching;

namespace PersonalBlog.Infrastructure.Caching;

public class RepositoryCacheService : IRepositoryCacheService
{
    private readonly IDistributedCache _distributedCache;

    public RepositoryCacheService(IDistributedCache distributedCache) => _distributedCache = distributedCache;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
        where T : class
    {
        string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (cachedValue is null)
        {
            return null;
        }

        T? value = JsonConvert.DeserializeObject<T>(cachedValue);

        return value;
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan absouluteExpireTime,
        CancellationToken cancellationToken
        )
        where T : class
    {
        string cachedValue = JsonConvert.SerializeObject(value);

        var cacheEntryOptions = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = absouluteExpireTime,
            //SlidingExpiration = unusedExpireTime
        };

        await _distributedCache.SetStringAsync(key, cachedValue, cacheEntryOptions, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}