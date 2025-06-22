namespace RecipeManagementSystem.Shared.Kafka;

public interface IKafkaProducer
{
    Task ProduceAsync<TValue>(string topic, TValue value, CancellationToken cancellationToken = default);
}