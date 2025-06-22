using RecipeManagementSystem.Notification.Contracts;

namespace RecipeManagementSystem.Notification.Services;

public class HttpRequestSender(IHttpClientFactory factory) : IHttpRequestSender
{
    public async Task PublishRecipeAsync(string recipeId)
    {
        var httpClient = factory.CreateClient(nameof(HttpRequestSender));

        var response = await httpClient.GetAsync($"api/recipe/publish-now/{recipeId}");
        response.EnsureSuccessStatusCode();
    }
}