using MediatR;
using RecipeManagementSystem.Domain.Common;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.GetAllRecipes;

public record GetAllRecipesQuery(FilterParams Filter) : IRequest<Result<PagedList<Recipe>>>;