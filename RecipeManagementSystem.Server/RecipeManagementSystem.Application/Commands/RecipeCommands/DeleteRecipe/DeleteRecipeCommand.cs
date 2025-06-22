using MediatR;
using RecipeManagementSystem.Domain.Common;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.DeleteRecipe;

public record DeleteRecipeCommand(string Id) : IRequest<Result>;