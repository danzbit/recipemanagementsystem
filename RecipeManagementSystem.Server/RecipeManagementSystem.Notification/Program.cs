using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Simpl;
using RecipeManagementSystem.Application.Kafka;
using RecipeManagementSystem.Import.Configurations;
using RecipeManagementSystem.Notification.Contracts;
using RecipeManagementSystem.Notification.Handlers;
using RecipeManagementSystem.Notification.Services;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Kafka;
using RecipeManagementSystem.Shared.Models;
using RecipeManagementSystem.Shared.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<ExternalApiOptions>(
    builder.Configuration.GetSection(ExternalApiOptions.ExternalApi));

builder.Services.Configure<KafkaOptions>(
    builder.Configuration.GetSection(KafkaOptions.Kafka));

builder.Services.AddHttpClient(nameof(HttpRequestSender), (sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<ExternalApiOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddQuartz(q =>
{
    q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();

    q.UsePersistentStore(options =>
    {
        q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 10; });
        options.RetryInterval = TimeSpan.FromSeconds(15);
        options.UseSqlServer(sqlServer =>
        {
            sqlServer.ConnectionString = builder.Configuration.GetConnectionString("Quartz");
            sqlServer.TablePrefix = "QRTZ_";
        });
        options.UseProperties = true;

        options.UseNewtonsoftJsonSerializer();
    });
});

builder.Services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

builder.Services.AddSingleton<IScheduler>(provider =>
{
    var schedulerFactory = provider.GetRequiredService<ISchedulerFactory>();
    var scheduler = schedulerFactory.GetScheduler().Result;
    return scheduler;
});

builder.Services.AddTransient<IHttpRequestSender, HttpRequestSender>();

var options = builder.Configuration.GetSection(KafkaOptions.Kafka).Get<KafkaOptions>();

if (options != null)
{
    builder.Services.AddKafkaProducer();

    builder.Services.AddKafkaConsumer<PublishRecipeMessage, PublishRecipeHandler>(
        bootstrapServer: options.BootstrapServer,
        topic: KafkaTopics.PublishRecipe,
        groupId: options.GroupId);

    builder.Services.AddKafkaConsumer<ResendInviteMessage, ResendInviteHandler>(
        bootstrapServer: options.BootstrapServer,
        topic: KafkaTopics.ResendInvites,
        groupId: options.GroupId);
    
    builder.Services.AddKafkaConsumer<CancelResendInviteMessage, CancelResendInviteHandler>(
        bootstrapServer: options.BootstrapServer,
        topic: KafkaTopics.ResendInviteCancel,
        groupId: options.GroupId);
    
    builder.Services.AddKafkaConsumer<AutoExpireInviteMessage, AutoExpireInviteHandler>(
        bootstrapServer: options.BootstrapServer,
        topic: KafkaTopics.ExpiredInvite,
        groupId: options.GroupId);
    
    builder.Services.AddKafkaConsumer<CancelAutoExpireInviteMessage, CancelAutoExpireInviteHandler>(
        bootstrapServer: options.BootstrapServer,
        topic: KafkaTopics.ExpiredInviteCancel,
        groupId: options.GroupId);
}

var host = builder.Build();
host.Run();