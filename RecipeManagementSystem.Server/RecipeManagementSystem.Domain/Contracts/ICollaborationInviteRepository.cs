using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Domain.Contracts;

public interface ICollaborationInviteRepository
{
    Task<Result<CollaborationInvite>> GetPendingInviteAsync(Guid recipeId, Guid inviteeId);
}