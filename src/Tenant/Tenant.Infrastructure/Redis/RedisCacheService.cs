using System.Text.Json;
using Common.Cache;
using Common.User;
using StackExchange.Redis;

namespace Tenant.Infrastructure.Redis;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer  _redis;
    private readonly IUser _user;

    public RedisCacheService(IConnectionMultiplexer  redis, IUser user)
    {
        _redis = redis;
        _user = user;
    }

    public async Task Insert(string key, string value)
    {
        key = $"{_user.TenantCode}_{key}";
        var database = _redis.GetDatabase(0);
        var val = await database.StringGetAsync(key);

        if (!string.IsNullOrEmpty(val))
            database.KeyDelete(key);
            
        database.StringSet(key, value);
    }

    public async Task<TResult> Get<TResult>(string key, Func<Task<TResult>> action)
    {
        key = $"{_user.TenantCode}_{key}";
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
        key = $"{_user.TenantCode}_{key}";
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
        key = $"{_user.TenantCode}_{key}";
        var database = _redis.GetDatabase(0);

        if (database.KeyExists(key))
            database.KeyDelete(key);

        return Task.CompletedTask;
    }
}