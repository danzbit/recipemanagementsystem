using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using RecipeManagementSystem.Application.Contracts;
using RecipeManagementSystem.Application.Hubs;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;
using RecipeManagementSystem.Shared.Pdf;

namespace RecipeManagementSystem.Application.Caching;

public class PdfStatusMemoryCache(IMemoryCache cache, IHubContext<PdfStatusHub, IHubClient> hubContext) : BaseMemoryCache(cache)
{
    private readonly IMemoryCache _cache = cache;

    public async Task<Result> UpdateStatus(string id, Action<PdfStatus> update)
    {
        if (_cache.TryGetValue(id, out PdfStatus? status))
        {
            update(status);
            Set(id, status);

            var connectionId = PdfStatusHub.GetConnectionIdForUser(status.RecipeId.ToString());

            if (connectionId is null)
            {
                return Result.Failure(HubErrors.RecipeNotConnected);    
            }
        
            await hubContext.Clients.Client(connectionId).PdfStatusUpdated(status);
            
            return Result.Success();
        }
        
        return Result.Failure(CacheErrors.ItemNotExistIntoCache);
    }
}