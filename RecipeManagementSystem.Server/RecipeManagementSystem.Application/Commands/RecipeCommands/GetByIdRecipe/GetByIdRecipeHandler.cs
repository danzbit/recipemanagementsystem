using AutoMapper;
using MediatR;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.GetByIdRecipe;

public class GetByIdRecipeHandler(IRecipeRepository repository, IMapper mapper)
    : IRequestHandler<GetByIdRecipeQuery, Result<RecipeDto>>
{
    public async Task<Result<RecipeDto>> Handle(GetByIdRecipeQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || !Guid.TryParse(request.Id, out Guid recipeId))
        {
            return Result<RecipeDto>.Failure(RequestErrors.InvalidId);
        }
        
        var result = await repository.GetById(recipeId);
        
        var mappedResult = mapper.Map<RecipeDto>(result.Value);
        
        return result.IsSuccess
            ? Result<RecipeDto>.Success(mappedResult)
            : Result<RecipeDto>.Failure(result.Error);
    }
}