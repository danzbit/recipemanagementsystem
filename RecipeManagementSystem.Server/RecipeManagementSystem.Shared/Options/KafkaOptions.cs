namespace RecipeManagementSystem.Shared.Options;

public class KafkaOptions
{
    public const string Kafka = "Kafka";

    public string BootstrapServer { get; set; } = string.Empty;
    
    public string GroupId { get; set; } = string.Empty;
    
    public string RequestTopic { get; set; } = string.Empty;
    
    public GroupIdsOption GroupIds { get; set; } = new();
}