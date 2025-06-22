namespace RecipeManagementSystem.Shared.Errors;

public static class AuthErrors
{
    public static readonly Error UserAlreadyExist = new(ErrorCode.UserAlreadyExist, "User already exist.");

    public static readonly Error UserNotFound = new(ErrorCode.UserNotFound, "User not found.");

    public static readonly Error Unauthorized = new(ErrorCode.Unauthorized, "Unauthorized.");

    public static readonly Error UserCreation = new(ErrorCode.UserCreation, "Failed to create user.");
}