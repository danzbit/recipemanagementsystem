using Microsoft.AspNetCore.Identity;
using RecipeManagementSystem.Domain.Common;
using RecipeManagementSystem.Domain.Common.Requests;
using RecipeManagementSystem.Domain.Common.Responses;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Contracts;

public interface IAuthService
{
    string GenerateAccessToken(IdentityUser user);

    Task<Result<List<string>>> Register(RegistrationRequest userModel);
    
    Task<Result<LoginResponse>> Login(LoginRequest loginRequest);
}