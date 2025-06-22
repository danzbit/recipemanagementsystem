namespace RecipeManagementSystem.Shared.Errors;

public static class CollaborationInviteErrors
{
    public static readonly Error CollaborationInviteAlreadySent = new(ErrorCode.CollaborationInviteAlreadySent, "Invite already sent to this user.");
    
    public static readonly Error CannotAcceptInvite = new(ErrorCode.CannotAcceptInvite, "You cannot respond to this invite."); 
    
    public static readonly Error InviteExpired = new(ErrorCode.InviteIsExpired, "Your invite has expired."); 
}