using AutoMapper;
using MediatR;
using RecipeManagementSystem.Application.Caching;
using RecipeManagementSystem.Application.Kafka;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.ScheduleRecipePublish;

public class ScheduleRecipePublishHandler(PublishRecipeMemoryCache cache, IKafkaProducer producer, IMapper mapper)
    : IRequestHandler<ScheduleRecipePublishCommand, Result>
{
    public async Task<Result> Handle(ScheduleRecipePublishCommand request, CancellationToken cancellationToken)
    {
        var mappedRecipe = mapper.Map<Recipe>(request.Recipe);
        mappedRecipe.Id = Guid.NewGuid();
        
        if (request.Recipe.ScheduledTime > 0)
        {
            var message = new PublishRecipeMessage
            {
                RecipeId = mappedRecipe.Id,
                ScheduledTime = DateTimeOffset.UtcNow.AddMinutes(request.Recipe.ScheduledTime)
            };

            await producer.ProduceAsync(KafkaTopics.PublishRecipe, message, cancellationToken);
            cache.PublishRecipe(mappedRecipe, TimeSpan.FromMinutes(request.Recipe.ScheduledTime + 5));
        
            return Result.Success();
        }
     
        return Result.Failure(RequestErrors.InvalidDate);
    }
}