using Microsoft.Extensions.Options;
using RecipeManagementSystem.Import.Configurations;
using RecipeManagementSystem.Import.Contracts;
using RecipeManagementSystem.Import.Services;
using RecipeManagementSystem.Shared.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<ExternalApiOptions>(
    builder.Configuration.GetSection(ExternalApiOptions.ExternalApi));

builder.Services.Configure<KafkaOptions>(
    builder.Configuration.GetSection(KafkaOptions.Kafka));

builder.Services.AddHttpClient(nameof(ApiPdfStatusUpdater), (sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<ExternalApiOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddTransient<IPdfStatusUpdater, ApiPdfStatusUpdater>();
builder.Services.AddTransient<IPdfService, PdfService>();
builder.Services.AddHostedService<KafkaConsumer>();

var host = builder.Build();
host.Run();