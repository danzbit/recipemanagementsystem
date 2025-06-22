namespace RecipeManagementSystem.Shared.Errors;

public static class KafkaErrors
{
    public static readonly Error NotFoundHandler = new(ErrorCode.NotFoundHandler, "Handler not found.");
    
    public static readonly Error DeserializationFailed = new(ErrorCode.DeserializationFailed, "Failed to deserialize message.");
}