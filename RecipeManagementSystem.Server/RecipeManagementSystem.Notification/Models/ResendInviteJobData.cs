namespace RecipeManagementSystem.Notification.Models;

public class ResendInviteJobData
{
    public string UserId { get; set; }
    
    public Guid InviteId { get; set; }
}