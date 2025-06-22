using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RecipeManagementSystem.Shared.Kafka;

public class KafkaConsumer<TMessage>(
    string topic,
    ConsumerConfig config,
    KafkaDispatcher dispatcher,
    ILogger<KafkaConsumer<TMessage>> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<string, string>(config)
            .SetErrorHandler((_, e) => logger.LogError("Error: {Arg2Reason}", e.Reason)).Build();
        consumer.Subscribe(topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                ConsumeResult<string, string>? cr;

                try
                {
                    cr = await Task.Run(() => consumer.Consume(stoppingToken), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    logger.LogWarning("Kafka consumption cancelled.");
                    break;
                }

                if (cr == null || cr.IsPartitionEOF)
                {
                    continue;
                }

                var success = false;
                var attempt = 0;
                const int maxAttempts = 3;

                while (!success && attempt < maxAttempts)
                {
                    try
                    {
                        attempt++;
                        await dispatcher.DispatchAsync<TMessage>(cr.Message.Value, stoppingToken);
                        consumer.Commit(cr);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("[ERROR] Handling {Topic} message failed (attempt {Attempt}): {ExMessage}",
                            cr.Topic, attempt,
                            ex.Message);
                        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                    }
                }

                if (!success)
                {
                    logger.LogWarning("[FAILURE] Handling message permanently failed after {MaxAttempts} attempts.",
                        maxAttempts);
                }
            }
            catch (ConsumeException ex)
            {
                logger.LogError("[CONSUME ERROR] {ErrorReason}", ex.Error.Reason);
            }
            catch (Exception ex)
            {
                logger.LogError("[UNEXPECTED ERROR] {ExMessage}", ex.Message);
            }
        }
        
        consumer.Close();

        logger.LogWarning("Kafka consumer stopped.");
    }
}