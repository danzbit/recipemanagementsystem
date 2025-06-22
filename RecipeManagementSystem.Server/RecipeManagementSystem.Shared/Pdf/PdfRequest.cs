using RecipeManagementSystem.Shared.DTOs;

namespace RecipeManagementSystem.Shared.Pdf;

public class PdfRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public RecipeDto? Recipe { get; set; }

    public Guid RecipeId { get; set; } 
}