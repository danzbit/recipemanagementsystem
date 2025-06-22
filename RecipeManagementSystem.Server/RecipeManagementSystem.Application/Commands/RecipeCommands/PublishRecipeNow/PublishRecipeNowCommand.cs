using MediatR;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.PublishRecipeNow;

public record PublishRecipeNowCommand(string RecipeId) : IRequest<Result>;