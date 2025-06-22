namespace RecipeManagementSystem.Shared.Invites;

public class ResendInviteExecutionMessage
{
    public string UserId { get; set; }

    public Guid InviteId { get; set; }
}