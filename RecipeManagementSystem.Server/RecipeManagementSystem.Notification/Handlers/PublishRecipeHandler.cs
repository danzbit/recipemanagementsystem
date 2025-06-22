using Quartz;
using RecipeManagementSystem.Notification.Jobs;
using RecipeManagementSystem.Notification.Models;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Notification.Handlers;

public class PublishRecipeHandler(ISchedulerFactory schedulerFactory, ILogger<PublishRecipeHandler> logger)
    : BaseJobHandler<PublishRecipeJob>, IKafkaHandler<PublishRecipeMessage>
{
    public async Task<Result> HandleAsync(PublishRecipeMessage message, CancellationToken cancellationToken)
    {
        var jobData = new PublishRecipeJobData
        {
            RecipeId = message.RecipeId
        };

        const string prefix = "publish-recipe";
        var job = BuildJob(message.Id, jobData, prefix);

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger-{prefix}-{message.Id}")
            .StartAt(message.ScheduledTime.UtcDateTime)
            .Build();

        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        await scheduler.ScheduleJob(job, trigger, cancellationToken);

        logger.LogInformation("Scheduled PublishRecipeJob for RecipeId={RecipeId} at {Time}",
            message.RecipeId, message.ScheduledTime);
        
        return Result.Success();
    }
}