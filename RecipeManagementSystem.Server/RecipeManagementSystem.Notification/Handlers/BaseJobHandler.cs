using Quartz;
using RecipeManagementSystem.Notification.Extensions;

namespace RecipeManagementSystem.Notification.Handlers;

public abstract class BaseJobHandler<TJob> where TJob : IJob
{
    protected IJobDetail BuildJob<T>(Guid messageId, T jobData, string prefix)
    {
        var map = new JobDataMap();
        map.PutTyped(jobData);

        return JobBuilder.Create<TJob>()
            .WithIdentity($"job-{prefix}-{messageId}")
            .SetJobData(map)
            .Build();
    }
}