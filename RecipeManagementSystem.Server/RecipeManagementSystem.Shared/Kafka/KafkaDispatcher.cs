using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Invites;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Shared.Kafka;

public class KafkaDispatcher(IServiceScopeFactory scopeFactory)
{
    public async Task<Result> DispatchAsync<T>(string message, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<T>>();

        var obj = JsonSerializer.Deserialize<T>(message);
        if (obj == null)
        {
            return Result.Failure(KafkaErrors.DeserializationFailed);
        }

        await handler.HandleAsync(obj, cancellationToken);
        return Result.Success();    
    }
}