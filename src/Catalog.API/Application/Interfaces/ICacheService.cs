namespace Catalog.API.Application.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, Expiration expiry);
    Task SetAsync<T>(string key, T value, Expiration expiry);
    Task RemoveAsync(string key);
}