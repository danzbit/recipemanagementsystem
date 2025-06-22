using Quartz;
using RecipeManagementSystem.Notification.Constants;
using RecipeManagementSystem.Notification.Jobs;
using RecipeManagementSystem.Notification.Models;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Notification.Handlers;

public class AutoExpireInviteHandler(ISchedulerFactory schedulerFactory, ILogger<AutoExpireInviteHandler> logger)
    : BaseJobHandler<AutoExpireInviteJob>, IKafkaHandler<AutoExpireInviteMessage>
{
    public async Task<Result> HandleAsync(AutoExpireInviteMessage message, CancellationToken cancellationToken)
    {
        var jobData = new AutoExpireInviteJobData
        {
            InviteId = message.InviteId
        };
        
        var job = BuildJob(message.InviteId, jobData, JobPrefixes.ExpiredInvite);

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger-{JobPrefixes.ExpiredInvite}-{message.InviteId}")
            .StartAt(message.ExpiresOn.UtcDateTime)
            .Build();

        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        await scheduler.ScheduleJob(job, trigger, cancellationToken);

        logger.LogInformation("Scheduled AutoExpireInviteJob for InviteId={InviteId} at {Time}",
            message.InviteId, message.ExpiresOn);
        
        return Result.Success();
    }
}