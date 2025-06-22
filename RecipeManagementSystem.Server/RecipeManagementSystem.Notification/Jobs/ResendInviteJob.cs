using Quartz;
using RecipeManagementSystem.Application.Kafka;
using RecipeManagementSystem.Notification.Extensions;
using RecipeManagementSystem.Notification.Models;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Notification.Jobs;

public class ResendInviteJob(IKafkaProducer producer) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var data = context.MergedJobDataMap.GetTyped<ResendInviteJobData>();

        var message = new ResendInviteExecutionMessage
        {
            UserId = data.UserId,
            InviteId = data.InviteId
        };

        await producer.ProduceAsync(KafkaTopics.ResendInvitesExecution, message);
    }
}