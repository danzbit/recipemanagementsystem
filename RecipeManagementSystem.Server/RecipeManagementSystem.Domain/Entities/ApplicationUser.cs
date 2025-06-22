using Microsoft.AspNetCore.Identity;

namespace RecipeManagementSystem.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Recipe> OwnedRecipes { get; set; } =  new List<Recipe>();
    public ICollection<RecipeCollaborator> Collaborations { get; set; } = new List<RecipeCollaborator>();
    public ICollection<CollaborationInvite> SentInvites { get; set; } = new List<CollaborationInvite>();
    public ICollection<CollaborationInvite> ReceivedInvites { get; set; } = new List<CollaborationInvite>();
}