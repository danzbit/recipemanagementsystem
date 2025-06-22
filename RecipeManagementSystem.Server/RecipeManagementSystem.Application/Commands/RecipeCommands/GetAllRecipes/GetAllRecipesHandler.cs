using MediatR;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.GetAllRecipes;

public class GetAllRecipesHandler(IRecipeRepository repository) : IRequestHandler<GetAllRecipesQuery, Result<PagedList<Recipe>>>
{
    public async Task<Result<PagedList<Recipe>>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllRecipesAsync(request.Filter);
        return result;
    }
}