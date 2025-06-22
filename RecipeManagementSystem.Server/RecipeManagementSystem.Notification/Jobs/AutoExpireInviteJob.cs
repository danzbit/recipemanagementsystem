using Quartz;
using RecipeManagementSystem.Application.Kafka;
using RecipeManagementSystem.Notification.Extensions;
using RecipeManagementSystem.Notification.Models;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Notification.Jobs;

public class AutoExpireInviteJob(IKafkaProducer producer) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var data = context.MergedJobDataMap.GetTyped<AutoExpireInviteJobData>();
        
        var message = new AutoExpireInviteExecutionMessage
        {
            InviteId = data.InviteId
        };

        await producer.ProduceAsync(KafkaTopics.ExpiredInviteExecution, message);
    }
}