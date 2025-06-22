using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Shared.Kafka;

public interface IKafkaHandler<in TMessage>
{
    Task<Result> HandleAsync(TMessage message, CancellationToken cancellationToken);
}