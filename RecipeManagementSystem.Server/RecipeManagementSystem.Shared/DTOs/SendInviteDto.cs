namespace RecipeManagementSystem.Shared.DTOs;

public class SendInviteDto
{
    public string RecipeId { get; set; }
    
    public string InviteeId { get; set; }
    
    public string InviterId { get; set; } 
}