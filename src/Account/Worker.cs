namespace Account;

using QueueClient;
using Models;
using Database;

public class Worker(ILogger<Worker> logger, 
    IEventConsumer consumer,
    IEventProducer producer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        logger.LogInformation("Waiting for events from topic 'createAccount`");
        await consumer.Consume(async accountCreateEvent =>
        {
            var account = accountCreateEvent.Parse<Account>();
            Database.Append(Store.ACCOUNTS, account.Username);
            await producer.ProduceAsync("accountCreated", account);
        }, stoppingToken);
    }
}
