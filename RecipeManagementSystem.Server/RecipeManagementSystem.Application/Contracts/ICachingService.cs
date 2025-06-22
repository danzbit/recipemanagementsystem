namespace RecipeManagementSystem.Application.Contracts;

public interface ICachingService
{
    void Set<T>(string key, T value, TimeSpan? expiry = null);

    T? Get<T>(string key);
}