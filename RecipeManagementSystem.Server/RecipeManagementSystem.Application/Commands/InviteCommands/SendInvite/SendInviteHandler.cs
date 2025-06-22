using AutoMapper;
using MediatR;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.InviteCommands.SendInvite;

public class SendInviteHandler(
    IGenericRepository<CollaborationInvite> repository,
    ICollaborationInviteRepository collaborationInviteRepository,
    IResendInviteService resendInviteService,
    IMapper mapper)
    : IRequestHandler<SendInviteCommand, Result>
{
    public async Task<Result> Handle(SendInviteCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.SendInvite.RecipeId) || !Guid.TryParse(request.SendInvite.RecipeId, out Guid recipeId))
        {
            return Result.Failure(RequestErrors.InvalidId);
        }
        
        if (string.IsNullOrWhiteSpace(request.SendInvite.InviteeId) || !Guid.TryParse(request.SendInvite.InviteeId, out Guid inviteeId))
        {
            return Result.Failure(RequestErrors.InvalidId);
        }
        
        if (string.IsNullOrWhiteSpace(request.SendInvite.InviterId) || !Guid.TryParse(request.SendInvite.InviterId, out Guid inviterId))
        {
            return Result.Failure(RequestErrors.InvalidId);
        }
        
        var existingInvite = await collaborationInviteRepository.GetPendingInviteAsync(recipeId, inviteeId);

        if (existingInvite.IsSuccess)
        {
            return Result.Failure(CollaborationInviteErrors.CollaborationInviteAlreadySent);
        }
        
        var invite =  mapper.Map<CollaborationInvite>(request.SendInvite);
        var inviteId = Guid.NewGuid();
        invite.Id = inviteId;
        
        var result = await repository.AddAsync(invite);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return await resendInviteService.ScheduleJobForResendingInvite(new ResendInviteMessage
        {
            InviteId = inviteId,
            UserId = invite.InviterId,
        });
    }
}