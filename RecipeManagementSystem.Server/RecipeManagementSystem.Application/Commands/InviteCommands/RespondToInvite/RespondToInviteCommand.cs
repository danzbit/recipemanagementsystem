using MediatR;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.InviteCommands.RespondToInvite;

public record RespondToInviteCommand(RespondToInviteDto RespondToInvite) : IRequest<Result>;

