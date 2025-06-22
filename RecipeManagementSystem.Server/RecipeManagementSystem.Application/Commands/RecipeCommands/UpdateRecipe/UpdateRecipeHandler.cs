using AutoMapper;
using MediatR;
using RecipeManagementSystem.Domain.Common;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.UpdateRecipe;

public class UpdateRecipeHandler(IRecipeRepository repository, IMapper mapper) : IRequestHandler<UpdateRecipeCommand, Result>
{
    public async Task<Result> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || !Guid.TryParse(request.Id, out Guid recipeId))
        {
            return Result.Failure(RequestErrors.InvalidId);
        }
        
        var recipe = mapper.Map<Recipe>(request.Recipe);
        recipe.Id = recipeId;
        
        var result = await repository.UpdateAsync(recipe); 
        return result;
    }
}