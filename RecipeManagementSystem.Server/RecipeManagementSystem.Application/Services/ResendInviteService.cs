using RecipeManagementSystem.Application.Caching;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Application.Kafka;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Services;

public class ResendInviteService(
    IKafkaProducer producer,
    IGenericRepository<CollaborationInvite> repository,
    ResendInviteMemoryCache cache) : IResendInviteService
{
    public async Task<Result> ScheduleJobForResendingInvite(ResendInviteMessage message)
    {
        var invite = await repository.GetById(message.InviteId);

        if (invite.IsFailure)
        {
            return Result.Failure(invite.Error);
        }

        await producer.ProduceAsync(KafkaTopics.ResendInvites, message);

        cache.SetScheduledResendInviteJob(message.InviteId.ToString(), message.ScheduledTime.Offset);
        return Result.Success();
    }

    public async Task<Result> CancelScheduledJobForResendingInvite(string inviteId)
    {
        if (cache.Get<string>(inviteId) is null)
        {
            return Result.Failure(RequestErrors.InvalidId);
        }

        var message = new CancelResendInviteMessage
        {
            InviteId = Guid.Parse(inviteId)
        };

        await producer.ProduceAsync(KafkaTopics.ResendInviteCancel, message);
        cache.RemoveScheduledResendInviteJob(inviteId);

        return Result.Success();
    }
}