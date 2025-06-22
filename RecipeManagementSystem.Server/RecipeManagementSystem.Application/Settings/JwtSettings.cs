using System.ComponentModel.DataAnnotations;

namespace RecipeManagementSystem.Application.Settings;

public class JwtSettings
{
    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required] 
    public string Audience { get; set; } = string.Empty;

    [Required] 
    public string SecretKey { get; set; } = string.Empty;
}