namespace Application.Caching;

public interface IRepositoryCacheService
{
    Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken)
        where T : class;

    public Task SetAsync<T>(
        string key, T value,
        TimeSpan absouluteExpireTime,
        CancellationToken cancellationToken
        )
        where T : class;

    Task RemoveAsync(string key, CancellationToken cancellationToken);

    //Task RemoveAllByPrefix(string prefixKey, CancellationToken cancellationToken);
}