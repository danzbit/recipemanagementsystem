namespace RecipeManagementSystem.Shared.Errors;

public static class JobErrors
{
    public static readonly Error JobNotFound = new(ErrorCode.JobNotFound, "Job doesn't exist.");
}