using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using RecipeManagementSystem.Import.Contracts;
using RecipeManagementSystem.Shared.Models;
using RecipeManagementSystem.Shared.Options;
using RecipeManagementSystem.Shared.Pdf;

namespace RecipeManagementSystem.Import.Services;

public class KafkaConsumer(IOptions<KafkaOptions> settings, IPdfService pdfService, ILogger<KafkaConsumer> logger)
    : BackgroundService
{
    private readonly KafkaOptions _kafkaOptions = settings.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServer,
            GroupId = _kafkaOptions.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            EnablePartitionEof = true,
            SessionTimeoutMs = 10000,
            MaxPollIntervalMs = 300000,
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config)
            .SetErrorHandler((_, e) => logger.LogError("Error: {Arg2Reason}", e.Reason)).Build();
        consumer.Subscribe(_kafkaOptions.RequestTopic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                ConsumeResult<Ignore, string>? result;

                try
                {
                    result = consumer.Consume(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    logger.LogWarning("Kafka consumption cancelled.");
                    break;
                }

                if (result == null || result.IsPartitionEOF)
                    continue;

                PdfRequest? request = null;

                try
                {
                    request = JsonSerializer.Deserialize<PdfRequest>(result.Message.Value);
                    if (request == null)
                    {
                        throw new InvalidDataException("Deserialized request is null.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("[WARN] Failed to deserialize message: {ExMessage}", ex.Message);
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
                        await pdfService.GenerateAsync(request);
                        consumer.Commit(result);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("[ERROR] PDF generation failed (attempt {Attempt}): {ExMessage}", attempt,
                            ex.Message);
                        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                    }
                }

                if (!success)
                {
                    logger.LogWarning("[FAILURE] PDF generation permanently failed after {MaxAttempts} attempts.",
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

        logger.LogWarning("Kafka consumer stopped.");
    }
}