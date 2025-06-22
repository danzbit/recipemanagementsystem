using System.ComponentModel.DataAnnotations;

namespace RecipeManagementSystem.Domain.Common.Requests;

public class RegistrationRequest
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, ErrorMessage = "The username should be less than 100 characters")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string? Email { get; init; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string? Password { get; init; }
}