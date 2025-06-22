namespace RecipeManagementSystem.Shared.Invites;

public class PublishRecipeMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid RecipeId { get; set; }
    
    public DateTimeOffset ScheduledTime { get; set; }
}