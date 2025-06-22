using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Domain.Common.Requests;
using RecipeManagementSystem.Domain.Models;
using LoginRequest = RecipeManagementSystem.Domain.Common.Requests.LoginRequest;

namespace RecipeManagementSystem.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [Route("sign-up")]
    public async Task<IActionResult> Register(RegistrationRequest userModel)
    {
        var result = await authService.Register(userModel);

        return result.ToActionResult();
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("sign-in")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var result = await authService.Login(loginRequest);

        return result.ToActionResult();
    }
}
