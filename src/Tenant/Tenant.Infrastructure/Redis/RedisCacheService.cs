using System.Text.Json;
using Common.Cache;
using StackExchange.Redis;

namespace Tenant.Infrastructure.Redis;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer  _redis;

    public RedisCacheService(IConnectionMultiplexer  redis)
    {
        _redis = redis;
    }

    public async Task Insert(string key, string value)
    {
        var database = _redis.GetDatabase(0);
        var val = await database.StringGetAsync(key);

        if (!string.IsNullOrEmpty(val))
            database.KeyDelete(key);
            
        database.StringSet(key, value);
    }

    public async Task<TResult> Get<TResult>(string key, Func<Task<TResult>> action)
    {
        var database = _redis.GetDatabase(0);
        var value = await database.StringGetAsync(key);

        if (!string.IsNullOrEmpty(value))
            return JsonSerializer.Deserialize<TResult>(value);

        if (database.KeyExists(key) && string.IsNullOrEmpty(value))
            database.KeyDelete(key);
            
        var result = await action.Invoke();
        await Insert(key, JsonSerializer.Serialize(result));

        return result;
    }

    public async Task<List<TResult>> Get<TResult>(string key, Func<Task<List<TResult>>> action)
    {
        var database = _redis.GetDatabase(0);
        var isExist = database.KeyExists(key);

        if (isExist)
            return JsonSerializer.Deserialize<List<TResult>>(database.StringGet(key));
            
        var result = await action.Invoke();       
        await Insert(key, JsonSerializer.Serialize(result));

        return result;
    }

    public Task InvalidateKey(string key)
    {
        var database = _redis.GetDatabase(0);

        if (database.KeyExists(key))
            database.KeyDelete(key);

        return Task.CompletedTask;
    }
}