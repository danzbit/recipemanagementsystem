namespace RecipeManagementSystem.Shared.Errors;

public static class CacheErrors
{
    public static readonly Error ItemAlreadyAddedToCache = new(ErrorCode.ItemAlreadyAddedToCache, "Item already added to cache.");
    
    public static readonly Error ItemNotExistIntoCache = new(ErrorCode.ItemNotExistInoCache, "Item don't exist in the cache.");
}