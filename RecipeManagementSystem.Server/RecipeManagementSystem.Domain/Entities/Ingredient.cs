namespace RecipeManagementSystem.Domain.Entities;

public class Ingredient : BaseEntity
{
    public decimal Quantity { get; set; }

    public Product Product { get; set; } = null!;
    
    public Guid? RecipeId { get; set; }
}