using RecipeManagementSystem.Domain.Enums;

namespace RecipeManagementSystem.Domain.Entities;

public class CollaborationInvite : BaseEntity
{
    public Guid RecipeId { get; set; }
    
    public Recipe Recipe { get; set; }

    public string InviterId { get; set; }
    
    public ApplicationUser Inviter { get; set; }

    public string InviteeId { get; set; }
    
    public ApplicationUser Invitee { get; set; }

    public InviteStatus Status { get; set; } = InviteStatus.Pending;

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? RespondedAt { get; set; }
}