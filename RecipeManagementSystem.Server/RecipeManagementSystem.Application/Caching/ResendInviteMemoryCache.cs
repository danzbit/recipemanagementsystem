using Microsoft.Extensions.Caching.Memory;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Application.Caching;

public class ResendInviteMemoryCache(IMemoryCache cache) : BaseMemoryCache(cache)
{
    public Result SetScheduledResendInviteJob(string inviteId, TimeSpan scheduledTime)
    {
        var jobKey = $"job-resend-invite-{inviteId}";

        if (Get<string>(inviteId) is null)
        {
            Set(inviteId, jobKey, scheduledTime);
            return Result.Success();
        }

        return Result.Failure(CacheErrors.ItemAlreadyAddedToCache);
    }

    public Result RemoveScheduledResendInviteJob(string inviteId)
    {
        if (Get<string>(inviteId) is not null)
        {
            Remove(inviteId);
            return Result.Success();
        }
        
        return Result.Failure(CacheErrors.ItemNotExistIntoCache);
    }
}