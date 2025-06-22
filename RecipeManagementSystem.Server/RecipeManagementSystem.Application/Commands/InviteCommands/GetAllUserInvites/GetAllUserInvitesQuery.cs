using MediatR;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.InviteCommands.GetAllUserInvites;

public record GetAllUserInvitesQuery(string UserId) : IRequest<Result<IEnumerable<CollaborationInviteDto>>>;