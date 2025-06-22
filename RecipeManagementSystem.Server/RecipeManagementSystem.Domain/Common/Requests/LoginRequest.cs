using System.ComponentModel.DataAnnotations;

namespace RecipeManagementSystem.Domain.Common.Requests;

public class LoginRequest
{
    [Required(ErrorMessage = "Username or email is required.")]
    public string? UserNameOrEmail { get; init; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; init; }
}