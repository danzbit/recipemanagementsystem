namespace RecipeManagementSystem.Shared.DTOs;

public class CollaborationInviteDto
{
    public Guid Id { get; set; }
    
    public string InviteeEmail { get; set; }
    
    public string InviterEmail { get; set; }
    
    public string RecipeTitle { get; set; }
    
    public InviteStatusDto Status { get; set; }
    
    public DateTime SentAt { get; set; }
}