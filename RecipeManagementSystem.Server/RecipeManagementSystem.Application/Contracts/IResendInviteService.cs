using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Contracts;

public interface IResendInviteService
{
    Task<Result> ScheduleJobForResendingInvite(ResendInviteMessage message);

    Task<Result> CancelScheduledJobForResendingInvite(string inviteId);
}