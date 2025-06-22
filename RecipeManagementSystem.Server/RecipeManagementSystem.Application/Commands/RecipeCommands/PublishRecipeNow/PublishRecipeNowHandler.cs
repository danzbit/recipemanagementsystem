using MediatR;
using Microsoft.AspNetCore.SignalR;
using RecipeManagementSystem.Application.Caching;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Application.Hubs;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.PublishRecipeNow;

public class PublishRecipeNowHandler(IRecipeRepository repository, PublishRecipeMemoryCache cache,
    IHubContext<RecipeHub, IHubClient> hubContext)
    : IRequestHandler<PublishRecipeNowCommand, Result>
{
    public async Task<Result> Handle(PublishRecipeNowCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RecipeId) || !Guid.TryParse(request.RecipeId, out Guid recipeId))
        {
            return Result.Failure(RequestErrors.InvalidId);
        }
        
        var recipeFromCache = cache.Get<Recipe>(recipeId.ToString());

        if (recipeFromCache is null)
        {
            Result.Failure(EntityErrors.EntityNotFound);
        }

        var result = await repository.AddAsync(recipeFromCache);
        
        var connectionId = RecipeHub.GetConnectionIdForUser(recipeFromCache.OwnerId);

        if (connectionId is null)
        {
            return Result.Failure(HubErrors.UserNotConnected);
        }

        if (result.IsFailure)
        {
            await hubContext.Clients.Client(connectionId).RecipeCreated("Recipe didn't publish successfully.");
            return Result.Failure(result.Error);
        }
        
        await hubContext.Clients.Client(connectionId).RecipeCreated("Recipe published successfully.");
        
        return Result.Success();
    }
}