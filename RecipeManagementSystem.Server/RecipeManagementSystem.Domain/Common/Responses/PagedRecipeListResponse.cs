using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;

namespace RecipeManagementSystem.Domain.Common.Responses;

public record PagedRecipeListResponse(PagedList<Recipe> Recipes, int TotalPages);