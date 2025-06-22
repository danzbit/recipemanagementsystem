namespace RecipeManagementSystem.Shared.Errors;

public static class EntityErrors
{
    public static readonly Error EntityAlreadyExist = new(ErrorCode.EntityAlreadyExist, "Entity has already exist.");
    
    public static readonly Error EntityNotFound = new(ErrorCode.EntityNotFound, "Entity doesn't exist.");
    
    public static readonly Error AddFailed = new(ErrorCode.AddFailed, "Failed to save the entity.");
    
    public static readonly Error UpdateFailed = new(ErrorCode.UpdateFailed, "Update failed.");
    
    public static readonly Error DeleteFailed = new(ErrorCode.DeleteFailed, "Delete failed.");
}