using MediatR;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.InviteCommands.SendInvite;

public record SendInviteCommand(SendInviteDto SendInvite): IRequest<Result>;