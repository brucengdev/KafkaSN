using QueueClient;

namespace Account;

public class Worker(ILogger<Worker> logger, IEventConsumer consumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Waiting for events from topic 'createAccount`");
            var createAccountEvent = consumer.Consume();

            logger.LogInformation("event: " + createAccountEvent.Message);
        }
    }
}
