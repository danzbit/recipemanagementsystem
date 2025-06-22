namespace RecipeManagementSystem.Shared.DTOs;

public class IngredientDto
{
    public string? Id { get; set; }
    
    public decimal Quantity { get; set; }

    public string Product { get; set; }
    
    public Guid? RecipeId { get; set; }
    
    public Guid? ProductId { get; set; }
}   