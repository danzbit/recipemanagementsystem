namespace RecipeManagementSystem.Shared.DTOs;

public class RespondToInviteDto
{
    public string InviteId { get; set; }
    
    public string UserId { get; set; }
    
    public InviteStatusDto Status { get; set; }
}