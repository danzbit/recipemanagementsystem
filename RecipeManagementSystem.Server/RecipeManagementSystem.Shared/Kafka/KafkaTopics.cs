namespace RecipeManagementSystem.Application.Kafka;

public static class KafkaTopics
{
    public const string PdfRequests = "pdf-requests";
    
    public const string PublishRecipe = "publish-recipe";
    
    public const string ResendInvites = "resend-invites";
    
    public const string ResendInvitesExecution = "resend-invites-execution";
    
    public const string ResendInviteCancel = "resend-invite-cancel";
    
    public const string ExpiredInvite = "expired-invite";
    
    public const string ExpiredInviteExecution = "expired-invite-execution";
    
    public const string ExpiredInviteCancel = "expired-invite-cancel";
}