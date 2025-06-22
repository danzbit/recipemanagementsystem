using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManagementSystem.Application.Commands.InviteCommands.GetAllUserInvites;
using RecipeManagementSystem.Application.Commands.InviteCommands.RespondToInvite;
using RecipeManagementSystem.Application.Commands.InviteCommands.SendInvite;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class InviteController(IMediator mediator) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAllUserInvites(string userId)
    {
        var result = await mediator.Send(new GetAllUserInvitesQuery(userId));
        
        return result.ToActionResult();
    }

    [HttpPost("send-invite")]
    public async Task<IActionResult> SendInvite([FromBody] SendInviteDto dto)
    {
        var result = await mediator.Send(new SendInviteCommand(dto));
        
        return result.ToActionResult();
    }
    
    [HttpPost("respond-to-invite")]
    public async Task<IActionResult> RespondToInvite([FromBody] RespondToInviteDto dto)
    {
        var result = await mediator.Send(new RespondToInviteCommand(dto));
        
        return result.ToActionResult();
    }
}