namespace Application.Interfaces;

public interface IRedisCacheService
{
    public Task<T> GetCachedDataAsync<T>(string key);

    public Task SetCachedDataAsync<T>(string key, T data, int timeDelete);

    public Task RemoveCachedData(string key);
}