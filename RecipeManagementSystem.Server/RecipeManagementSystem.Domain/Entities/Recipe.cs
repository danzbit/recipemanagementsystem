namespace RecipeManagementSystem.Domain.Entities;

public class Recipe : BaseEntity
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public double Calorie { get; set; }

    public required RecipeCategory RecipeCategory { get; set; }

    public ICollection<Ingredient> Ingredients { get; set; } = [];
    
    public string OwnerId { get; set; }
    
    public ApplicationUser Owner { get; set; }
    
    public List<RecipeCollaborator> Collaborators { get; set; } = new();
    
    public ICollection<CollaborationInvite> Invites { get; set; } = new List<CollaborationInvite>();
}