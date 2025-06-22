using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Domain.Contracts;

public interface IRecipeRepository
{
    Task<Result<PagedList<Recipe>>> GetAllRecipesAsync(FilterParams recipeParams);

    Task<Result<Recipe>> GetById(Guid id);

    Task<Result> AddAsync(Recipe recipe);

    Task<Result> UpdateAsync(Recipe recipe);
}