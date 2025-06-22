namespace RecipeManagementSystem.Shared.Errors;

public static class RequestErrors
{
    public static readonly Error InvalidId = new(ErrorCode.InvalidId, "Invalid ID format.");
    
    public static readonly Error InvalidDate = new(ErrorCode.InvalidDate, "Invalid Date format.");
}