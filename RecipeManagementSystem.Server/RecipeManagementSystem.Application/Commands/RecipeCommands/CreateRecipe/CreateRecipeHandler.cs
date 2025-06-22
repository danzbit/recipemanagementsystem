using AutoMapper;
using MediatR;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.CreateRecipe;

public class CreateRecipeHandler(IRecipeRepository repository, IMapper mapper)
    : IRequestHandler<CreateRecipeCommand, Result>
{
    public async Task<Result> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = mapper.Map<Recipe>(request.Recipe);
        recipe.Id = Guid.NewGuid();
        
        foreach (var ingredient in recipe.Ingredients)
        {
            ingredient.RecipeId = recipe.Id;
        }
    
        return await repository.AddAsync(recipe);
    }
}