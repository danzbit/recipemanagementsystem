using Microsoft.EntityFrameworkCore;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Infrastructure.Data;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Infrastructure.Repositories;

public class RecipeRepository(ApplicationDbContext context) : IRecipeRepository
{
    public async Task<Result<PagedList<Recipe>>> GetAllRecipesAsync(FilterParams recipeParams)
    {
        var source = BuildRecipeQuery();

        var search = string.Empty;
        var category = string.Empty;

        if (!string.IsNullOrEmpty(recipeParams.Search))
        {
            search = recipeParams.Search;
        }

        if (!string.IsNullOrEmpty(recipeParams.Category))
        {
            category = recipeParams.Category;
        }

        if (!string.IsNullOrEmpty(search))
        {
            source = source.Where(r => r.Title != null && (r.Title.ToLower().StartsWith(search)
                                                           || r.Ingredients.Any(i =>
                                                               i.Product != null && i.Product.Name != null &&
                                                               i.Product != null &&
                                                               i.Product.Name.ToLower().Trim()
                                                                   .StartsWith(search.ToLower().Trim()))));
        }

        if (!string.IsNullOrEmpty(category))
        {
            source = source.Where(r => r.RecipeCategory.Name == category);
        }

        return Result<PagedList<Recipe>>.Success(
            await PagedList<Recipe>.CreateAsync(source, recipeParams.PageNumber, recipeParams.PageSize));
    }

    public async Task<Result<Recipe>> GetById(Guid id)
    {
        var data = await context.Recipes
            .Include(r => r.RecipeCategory)
            .Include(i => i.Ingredients)
            .ThenInclude(p => p.Product)
            .Include(i => i.Collaborators)
            .ThenInclude(u => u.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        return data == null
            ? Result<Recipe>.Failure(EntityErrors.EntityNotFound)
            : Result<Recipe>.Success(data);
    }

    public async Task<Result> AddAsync(Recipe recipe)
    {
        var existedRecipe = await context.Recipes.FirstOrDefaultAsync(r => r.Title == recipe.Title);

        if (existedRecipe is not null)
        {
            return Result.Failure(EntityErrors.EntityAlreadyExist);
        }

        context.RecipeCategories.Attach(recipe.RecipeCategory);
        await context.Recipes.AddAsync(recipe);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Result.Success()
            : Result.Failure(EntityErrors.AddFailed);
    }

    public async Task<Result> UpdateAsync(Recipe entity)
    {
        var existingRecipe = await context.Recipes
            .AsTracking()
            .Include(recipe => recipe.RecipeCategory)
            .Include(i => i.Ingredients)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(r => r.Id == entity.Id);

        if (existingRecipe is null)
        {
            return Result.Failure(EntityErrors.EntityNotFound);
        }

        context.Entry(existingRecipe).State = EntityState.Modified;

        existingRecipe.Title = entity.Title;
        existingRecipe.Description = entity.Description;
        existingRecipe.Calorie = entity.Calorie;

        if (entity.RecipeCategory?.Name != existingRecipe.RecipeCategory?.Name)
        {
            if (entity.RecipeCategory != null)
            {
                existingRecipe.RecipeCategory = new RecipeCategory { Name = entity.RecipeCategory.Name };
                context.RecipeCategories.Attach(existingRecipe.RecipeCategory);
            }
        }

        var updatedIngredientIds = new List<Guid>();

        foreach (var ingredient in entity.Ingredients)
        {
            var matchedIngredient = existingRecipe.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);

            if (matchedIngredient is not null)
            {
                matchedIngredient.Quantity = ingredient.Quantity;

                if (matchedIngredient.Product.Id == ingredient.Product.Id)
                {
                    var trackedProduct = context.Products
                                             .Local
                                             .FirstOrDefault(p => p.Id == ingredient.Product.Id)
                                         ?? await context.Products.FirstOrDefaultAsync(p =>
                                             p.Id == ingredient.Product.Id);

                    if (trackedProduct != null)
                    {
                        if (trackedProduct.Name != ingredient.Product.Name)
                        {
                            trackedProduct.Name = ingredient.Product.Name;
                            context.Entry(trackedProduct).State = EntityState.Modified;
                        }

                        matchedIngredient.Product = trackedProduct;
                    }
                    else
                    {
                        var newProduct = new Product
                        {
                            Id = ingredient.Product.Id,
                            Name = ingredient.Product.Name
                        };
                        context.Products.Attach(newProduct);
                        matchedIngredient.Product = newProduct;
                        context.Entry(newProduct).State = EntityState.Modified;
                    }
                }

                updatedIngredientIds.Add(matchedIngredient.Id);
            }
            else
            {
                var product = new Product { Id = Guid.NewGuid(), Name = ingredient.Product.Name };

                var newIng = new Ingredient
                {
                    Id = Guid.NewGuid(),
                    Quantity = ingredient.Quantity,
                    RecipeId = existingRecipe.Id,
                    Product = product
                };

                await context.Ingredients.AddAsync(newIng);

                updatedIngredientIds.Add(newIng.Id);
            }
        }

        var toRemove = existingRecipe.Ingredients
            .Where(i => !updatedIngredientIds.Contains(i.Id))
            .ToList();

        if (toRemove.Any())
        {
            context.Ingredients.RemoveRange(toRemove);
        }

        var result = await context.SaveChangesAsync();

        return result == 0
            ? Result.Failure(EntityErrors.UpdateFailed)
            : Result<Recipe>.Success(existingRecipe);
    }

    private IQueryable<Recipe> BuildRecipeQuery()
    {
        return context.Recipes.Include(r => r.RecipeCategory)
            .Include(i => i.Ingredients);
    }
}