namespace Common.Cache;

public interface ICacheService
{
    Task Insert(string key, string value);
    Task<TResult> Get<TResult>(string key, Func<Task<TResult>> action);
    Task<List<TResult>> Get<TResult>(string key, Func<Task<List<TResult>>> action);
    Task InvalidateKey(string key);
}