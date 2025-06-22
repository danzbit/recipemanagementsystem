using AutoMapper;
using MediatR;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Application.Extensions;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Enums;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.InviteCommands.RespondToInvite;

public class RespondToInviteHandler(
    IGenericRepository<CollaborationInvite> collaborationInviteRepository,
    IGenericRepository<RecipeCollaborator> recipeCollaboratorRepository,
    IResendInviteService resendInviteService,
    IMapper mapper)
    : IRequestHandler<RespondToInviteCommand, Result>
{
    public async Task<Result> Handle(RespondToInviteCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RespondToInvite.InviteId) ||
            !Guid.TryParse(request.RespondToInvite.InviteId, out Guid inviteId))
        {
            return Result.Failure(RequestErrors.InvalidId);
        }

        var invite = await collaborationInviteRepository.GetById(inviteId);

        if (invite.IsFailure)
        {
            return Result.Failure(invite.Error);
        }

        if  (invite.Value.InviteeId != request.RespondToInvite.UserId)
        {
            return Result.Failure(CollaborationInviteErrors.CannotAcceptInvite);
        }

        var convertedInviteStatus = request.RespondToInvite.Status.ConvertFromDto();
        invite.Value.Status = convertedInviteStatus;
        invite.Value.RespondedAt = DateTime.Now;

        var result = await collaborationInviteRepository.UpdateAsync(invite.Value);

        if (result.IsSuccess && convertedInviteStatus == InviteStatus.Accepted)
        {
            var collaborator = mapper.Map<RecipeCollaborator>(invite.Value);
            collaborator.RecipeId = invite.Value.RecipeId;
            var addedResult = await recipeCollaboratorRepository.AddAsync(collaborator);
            if (addedResult.IsFailure)
            {
                return Result.Failure(addedResult.Error);
            }
        }

        if (result.IsSuccess)
        {
            await resendInviteService.CancelScheduledJobForResendingInvite(invite.Value.Id.ToString());
        }

        return result;
    }
}