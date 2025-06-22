using Quartz;
using RecipeManagementSystem.Notification.Constants;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Notification.Handlers;

public class CancelResendInviteHandler(IScheduler scheduler, ILogger<CancelResendInviteHandler> logger)
    : IKafkaHandler<CancelResendInviteMessage>
{
    public async Task<Result> HandleAsync(CancelResendInviteMessage message, CancellationToken cancellationToken)
    {
        var jobKey = new JobKey($"job-{JobPrefixes.ResendInvite}-{message.InviteId}");
        if (await scheduler.CheckExists(jobKey, cancellationToken))
        {
            await scheduler.DeleteJob(jobKey, cancellationToken);
            logger.LogInformation($"Cancelled job {jobKey}");
            
            return Result.Success();
        }
        
        return Result.Failure(JobErrors.JobNotFound);
    }
}
