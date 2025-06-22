using Microsoft.AspNetCore.SignalR;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Application.Hubs;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Common.Handlers;

public class AutoExpireInviteExecutionHandler(IHubContext<InviteHub, IHubClient> hubContext, IGenericRepository<CollaborationInvite> repository)
    : IKafkaHandler<AutoExpireInviteExecutionMessage>
{
    public async Task<Result> HandleAsync(AutoExpireInviteExecutionMessage message, CancellationToken cancellationToken)
    {
        var invite = await repository.GetById(message.InviteId);

        if (invite.IsFailure)
        {
            return Result.Failure(invite.Error);
        }

        var messageToSend = $"Your invite from {invite.Value.Inviter.Email} has expired for {invite.Value.Recipe.Title}.";
        
        await hubContext.Clients.User(invite.Value.InviteeId).AutoExpireInvite(messageToSend);
        return Result.Success();
    }
}