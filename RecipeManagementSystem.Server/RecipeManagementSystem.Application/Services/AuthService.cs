using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Domain.Common;
using RecipeManagementSystem.Domain.Common.Requests;
using RecipeManagementSystem.Domain.Common.Responses;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Services;

public class AuthService(
    IConfiguration configuration,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : IAuthService
{
    public string GenerateAccessToken(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.Ticks.ToString(), ClaimValueTypes.Integer64),
        };

        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"] ?? string.Empty)),
                SecurityAlgorithms.HmacSha256)
        );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

        return encodedJwt;
    }

    public async Task<Result<List<string>>> Register(RegistrationRequest userModel)
    {
        if (userModel.Email is not null)
        {
            var userExist = await userManager.FindByEmailAsync(userModel.Email);

            if (userExist is not null)
            {
                return Result<List<string>>.Failure(AuthErrors.UserAlreadyExist);
            }
        }

        var user = new ApplicationUser
        {
            UserName = userModel.UserName,
            Email = userModel.Email,
        };

        if (userModel.Password is not null)
        {
            var result = await userManager.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
            {
                return Result<List<string>>.Failure(result.Errors.Select(e => e.Description).ToList(),
                    AuthErrors.UserCreation);
            }
        }

        return Result<List<string>>.Success();
    }

    public async Task<Result<LoginResponse>> Login(LoginRequest loginRequest)
    {
        if (loginRequest.UserNameOrEmail is null)
        {
            return Result<LoginResponse>.Failure();
        }

        var user = await userManager.FindByEmailAsync(loginRequest.UserNameOrEmail) ??
                   await userManager.FindByNameAsync(loginRequest.UserNameOrEmail);

        if (user is null)
        {
            return Result<LoginResponse>.Failure(AuthErrors.UserNotFound);
        }

        if (loginRequest.Password is not null)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);

            if (!result.Succeeded)
            {
                return Result<LoginResponse>.Failure(AuthErrors.Unauthorized);
            }
        }

        var token = GenerateAccessToken(user);

        return Result<LoginResponse>.Success(new LoginResponse(user.Id, token));
    }
}