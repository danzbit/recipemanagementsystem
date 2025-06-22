using Microsoft.AspNetCore.SignalR;
using RecipeManagementSystem.Application.Contracts;

namespace RecipeManagementSystem.Application.Hubs;

public class RecipeHub : Hub<IHubClient>
{
    private static readonly Dictionary<string, string> Users = new();

    public override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        if (!string.IsNullOrEmpty(userId))
        {
            Users[userId] = connectionId;
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var toRemove = Users.FirstOrDefault(kvp => kvp.Value == Context.ConnectionId);
        if (!string.IsNullOrEmpty(toRemove.Key))
            Users.Remove(toRemove.Key);

        return base.OnDisconnectedAsync(exception);
    }

    public static string? GetConnectionIdForUser(string userId)
    {
        return Users.TryGetValue(userId, out var connId) ? connId : null;
    }
}