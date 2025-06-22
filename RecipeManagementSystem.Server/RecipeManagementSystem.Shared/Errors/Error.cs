namespace RecipeManagementSystem.Shared.Errors;

public sealed record Error(ErrorCode Code, string? Description = null)
{
    public static readonly Error None = new(ErrorCode.None);
}