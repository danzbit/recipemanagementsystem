using Microsoft.AspNetCore.SignalR;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Application.Hubs;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Enums;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Common.Handlers;

public class ResendInviteExecutionHandler(IHubContext<InviteHub, IHubClient> hubContext, IGenericRepository<CollaborationInvite> repository)
    : IKafkaHandler<ResendInviteExecutionMessage>
{
    public async Task<Result> HandleAsync(ResendInviteExecutionMessage message, CancellationToken cancellationToken)
    {
        var invite = await repository.GetById(message.InviteId);

        if (invite.IsFailure)
        {
            return Result.Failure(invite.Error);
        }

        if (invite.Value.Status != InviteStatus.Pending && invite.Value.Status != InviteStatus.Expired)
        {
            return Result.Failure(CollaborationInviteErrors.CannotAcceptInvite);
        }
        
        if (invite.Value.Status == InviteStatus.Expired)
        {
            return Result.Failure(CollaborationInviteErrors.InviteExpired);
        }

        var messageToSend = $"You have a pending invite from {invite.Value.Inviter.Email}, please accept or reject either it will be expired.";
        
        await hubContext.Clients.User(message.UserId).ResendInvite(messageToSend);
        return Result.Success();
    }
}

