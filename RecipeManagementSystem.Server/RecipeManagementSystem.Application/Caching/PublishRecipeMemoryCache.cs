using Microsoft.Extensions.Caching.Memory;
using RecipeManagementSystem.Domain.Entities;

namespace RecipeManagementSystem.Application.Caching;

public class PublishRecipeMemoryCache(IMemoryCache cache) : BaseMemoryCache(cache)
{
    public void PublishRecipe(Recipe recipe, TimeSpan scheduledTime)
    {
        Set(recipe.Id.ToString(), recipe, scheduledTime);
    }
}