namespace RecipeManagementSystem.Domain.Entities;

public class RecipeCollaborator : BaseEntity
{
    public Guid RecipeId { get; set; }
    
    public Recipe Recipe { get; set; }

    public string UserId { get; set; }
    
    public ApplicationUser User { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.Now;
}