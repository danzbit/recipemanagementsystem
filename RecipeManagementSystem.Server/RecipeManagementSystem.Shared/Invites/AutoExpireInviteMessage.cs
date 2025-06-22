namespace RecipeManagementSystem.Shared.Invites;

public class AutoExpireInviteMessage
{   
    public Guid InviteId { get; set; }
    
    public DateTimeOffset ExpiresOn { get; set; } 
}