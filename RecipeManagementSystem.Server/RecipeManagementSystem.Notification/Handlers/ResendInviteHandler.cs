using Quartz;
using RecipeManagementSystem.Notification.Constants;
using RecipeManagementSystem.Notification.Jobs;
using RecipeManagementSystem.Notification.Models;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Notification.Handlers;

public class ResendInviteHandler(ISchedulerFactory schedulerFactory, ILogger<PublishRecipeHandler> logger)
    : BaseJobHandler<ResendInviteJob>, IKafkaHandler<ResendInviteMessage>
{
    public async Task<Result> HandleAsync(ResendInviteMessage message, CancellationToken cancellationToken)
    {
        var jobData = new ResendInviteJobData
        {
            UserId = message.UserId,
            InviteId = message.InviteId
        };

        var job = BuildJob(message.InviteId, jobData, JobPrefixes.ResendInvite);

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger-{JobPrefixes.ResendInvite}-{message.InviteId}")
            .StartAt(message.ScheduledTime.UtcDateTime)
            .Build();

        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        await scheduler.ScheduleJob(job, trigger, cancellationToken);

        logger.LogInformation("Scheduled ResendInviteJob for InviteId={InviteId} at {Time}",
            message.InviteId, message.ScheduledTime);

        return Result.Success();
    }
}