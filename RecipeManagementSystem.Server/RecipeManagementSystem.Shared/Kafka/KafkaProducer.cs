using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using RecipeManagementSystem.Shared.Options;

namespace RecipeManagementSystem.Shared.Kafka;

public class KafkaProducer : IKafkaProducer, IDisposable
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(IOptions<KafkaOptions> kafkaOptions)
    {
        var kafkaOptions1 = kafkaOptions.Value;
        _producer = new ProducerBuilder<Null, string>(new ProducerConfig
        {
            BootstrapServers = kafkaOptions1.BootstrapServer,
            MessageTimeoutMs = 5000,
            Acks = Acks.All,
            EnableIdempotence = true
        }).Build();
    }

    public async Task ProduceAsync<T>(string topic, T value, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(value);
        var message = new Message<Null, string> { Value = json };

        await _producer.ProduceAsync(topic, message, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush();
        _producer.Dispose();
    }
}