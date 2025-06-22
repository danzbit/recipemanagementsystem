using Microsoft.Extensions.Caching.Memory;
using RecipeManagementSystem.Application.Contracts;

namespace RecipeManagementSystem.Application.Caching;

public class BaseMemoryCache(IMemoryCache cache) : ICachingService
{
    private readonly TimeSpan _expiration = TimeSpan.FromMinutes(10);

    public void Set<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? _expiration
        };
        
        cache.Set(key, value, options);
    }

    public T? Get<T>(string key)
    {
        return cache.TryGetValue(key, out T? value) ? value : default;
    }
    
    public void Remove(string key)
    {
        cache.Remove(key);
    }
}