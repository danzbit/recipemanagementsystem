using MediatR;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Commands.RecipeCommands.ScheduleRecipePublish;

public record ScheduleRecipePublishCommand(RecipeDto Recipe) : IRequest<Result>;