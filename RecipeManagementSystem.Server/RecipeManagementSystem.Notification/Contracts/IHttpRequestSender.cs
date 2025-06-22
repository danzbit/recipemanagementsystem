namespace RecipeManagementSystem.Notification.Contracts;

public interface IHttpRequestSender
{
    Task PublishRecipeAsync(string recipeId);
}