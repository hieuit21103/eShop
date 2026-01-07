namespace Catalog.API.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public CacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, Expiration expiry)
    {
        var cachedValue = await GetAsync<T>(key);
        if (cachedValue != null)
        {
            return cachedValue;
        }

        var value = await factory();
        await SetAsync(key, value, expiry);
        return value;
    }

    public async Task SetAsync<T>(string key, T value, Expiration expiry)
    {
        var json = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, json, expiry);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}