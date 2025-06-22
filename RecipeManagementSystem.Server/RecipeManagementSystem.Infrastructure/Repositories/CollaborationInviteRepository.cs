using Microsoft.EntityFrameworkCore;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Enums;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Infrastructure.Data;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Infrastructure.Repositories;

public class CollaborationInviteRepository(ApplicationDbContext context) : ICollaborationInviteRepository
{
    public async Task<Result<CollaborationInvite>> GetPendingInviteAsync(Guid recipeId, Guid inviteeId)
    {
        var entity = await context.CollaborationInvites
            .FirstOrDefaultAsync(i =>
                i.RecipeId == recipeId && i.InviteeId == inviteeId.ToString() && i.Status == InviteStatus.Pending);

        return entity == null
            ? Result<CollaborationInvite>.Failure(EntityErrors.EntityNotFound)
            : Result<CollaborationInvite>.Success(entity);
    }
}