using MediatR;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.CreateRecipe;

public record CreateRecipeCommand(RecipeDto Recipe) : IRequest<Result>;