namespace RecipeManagementSystem.Shared.Invites;

public class ResendInviteMessage
{
    public string UserId { get; set; }

    public Guid InviteId { get; set; }
    
    public DateTimeOffset ScheduledTime { get; set; } = DateTime.UtcNow.AddMinutes(30);
}