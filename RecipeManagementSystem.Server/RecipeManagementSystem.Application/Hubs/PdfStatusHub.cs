using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using RecipeManagementSystem.Application.Contracts;

namespace RecipeManagementSystem.Application.Hubs;

public class PdfStatusHub : Hub<IHubClient>
{
    private static readonly ConcurrentDictionary<string, string> Recipes = new();

    public override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var userId = Context.GetHttpContext()?.Request.Query["recipeId"].ToString();
        if (!string.IsNullOrEmpty(userId))
        {
            Recipes[userId] = connectionId;
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // var toRemove = Recipes.FirstOrDefault(kvp => kvp.Value == Context.ConnectionId);
        // if (!string.IsNullOrEmpty(toRemove.Key))
        // {
        //     Recipes.TryRemove(toRemove.Key, out _);
        // }

        return base.OnDisconnectedAsync(exception);
    }

    public static string? GetConnectionIdForUser(string recipeId)
    {
        return Recipes.TryGetValue(recipeId, out var connId) ? connId : null;
    }
}