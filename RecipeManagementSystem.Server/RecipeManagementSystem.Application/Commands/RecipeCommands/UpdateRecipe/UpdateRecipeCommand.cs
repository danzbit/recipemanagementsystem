using MediatR;
using RecipeManagementSystem.Domain.Common;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.UpdateRecipe;

public record UpdateRecipeCommand(string Id, RecipeDto Recipe) : IRequest<Result>;