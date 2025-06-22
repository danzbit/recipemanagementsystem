using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManagementSystem.Application.Commands.RecipeCommands.CreateRecipe;
using RecipeManagementSystem.Application.Commands.RecipeCommands.DeleteRecipe;
using RecipeManagementSystem.Application.Commands.RecipeCommands.GetAllRecipes;
using RecipeManagementSystem.Application.Commands.RecipeCommands.GetByIdRecipe;
using RecipeManagementSystem.Application.Commands.RecipeCommands.PublishRecipeNow;
using RecipeManagementSystem.Application.Commands.RecipeCommands.ScheduleRecipePublish;
using RecipeManagementSystem.Application.Commands.RecipeCommands.UpdateRecipe;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipeController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllRecipes([FromQuery] FilterParams filterParams)
    {
        var result = await mediator.Send(new GetAllRecipesQuery(filterParams));

        return result.ToActionResultForPagedList();
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipeById(string id)
    {
        var result = await mediator.Send(new GetByIdRecipeQuery(id));

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromBody] RecipeDto dto)
    {
        var result = await mediator.Send(new CreateRecipeCommand(dto));

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("schedule")]
    public async Task<IActionResult> ScheduleRecipePublish([FromBody] RecipeDto dto)
    {
        var result = await mediator.Send(new ScheduleRecipePublishCommand(dto));

        return result.ToActionResult();
    }

    [HttpGet("publish-now/{recipeId}")]
    [AllowAnonymous]
    public async Task<IActionResult> PublishRecipeNow(string recipeId)
    {
        var result = await mediator.Send(new PublishRecipeNowCommand(recipeId));

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPut("{recipeId}")]
    public async Task<IActionResult> Update([FromBody] RecipeDto recipe, [FromRoute] string recipeId)
    {
        var result = await mediator.Send(new UpdateRecipeCommand(recipeId, recipe));

        return result.ToActionResult();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var result = await mediator.Send(new DeleteRecipeCommand(id));

        return result.ToActionResult();
    }
}