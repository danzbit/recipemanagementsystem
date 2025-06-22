using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RecipeManagementSystem.Shared.Kafka;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafkaConsumer<TMessage, THandler>(this IServiceCollection services,
        string bootstrapServer, string topic, string groupId)
        where THandler : class, IKafkaHandler<TMessage>
    {
        services.AddTransient<IKafkaHandler<TMessage>, THandler>();
        services.AddSingleton<KafkaDispatcher>();
        
        services.AddSingleton(provider =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServer,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                EnablePartitionEof = true,
                SessionTimeoutMs = 10000,
                MaxPollIntervalMs = 300000,
            };

            return new KafkaConsumer<TMessage>(
                topic,
                config,
                provider.GetRequiredService<KafkaDispatcher>(),
                provider.GetRequiredService<ILogger<KafkaConsumer<TMessage>>>()
            );
        });
        
        services.AddHostedService(provider =>
            provider.GetRequiredService<KafkaConsumer<TMessage>>());

        return services;
    }

    public static IServiceCollection AddKafkaProducer(this IServiceCollection services)
    {
        services.AddTransient<IKafkaProducer, KafkaProducer>();

        return services;
    }
}