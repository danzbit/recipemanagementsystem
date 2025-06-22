namespace RecipeManagementSystem.Shared.Errors;

public static class HubErrors
{
    public static readonly Error UserNotConnected = new(ErrorCode.UserNotConnected, "User not connected.");
    
    public static readonly Error RecipeNotConnected = new(ErrorCode.RecipeNotConnected, "Recipe not connected.");
}