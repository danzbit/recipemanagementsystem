using Quartz;
using RecipeManagementSystem.Notification.Contracts;
using RecipeManagementSystem.Notification.Extensions;
using RecipeManagementSystem.Notification.Models;

namespace RecipeManagementSystem.Notification.Jobs;

public class PublishRecipeJob(IHttpRequestSender httpRequestSender) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var data = context.MergedJobDataMap.GetTyped<PublishRecipeJobData>();
        await httpRequestSender.PublishRecipeAsync(data.RecipeId.ToString());
    }
}