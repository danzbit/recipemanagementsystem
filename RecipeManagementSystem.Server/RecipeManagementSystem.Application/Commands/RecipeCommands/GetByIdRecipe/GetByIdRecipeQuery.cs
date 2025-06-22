using MediatR;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.GetByIdRecipe;

public record GetByIdRecipeQuery(string Id) : IRequest<Result<RecipeDto>>;