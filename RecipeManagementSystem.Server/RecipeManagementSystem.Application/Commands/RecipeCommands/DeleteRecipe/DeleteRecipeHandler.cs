using MediatR;
using RecipeManagementSystem.Domain.Common;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.DeleteRecipe;

public class DeleteRecipeHandler(IGenericRepository<Recipe> repository) : IRequestHandler<DeleteRecipeCommand, Result>
{
    public async Task<Result> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || !Guid.TryParse(request.Id, out Guid recipeId))
        {
            return Result.Failure(RequestErrors.InvalidId);
        }
        
        var result = await repository.DeleteAsync(recipeId);
        return result;
    }
}