namespace Reporting;

using QueueClient;
using Models;
using Database;

public class Worker(ILogger<Worker> logger, 
    IEventConsumer consumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Waiting for events from topic `postCreated`");
            var receivedEvent = consumer.Consume();

            var post = receivedEvent.Parse<Post>();

            var date = post.SentTime;

            var fileName = $"reports/post-count-{date:yyyy-MM-dd}.txt";
            var fileContent = Database.ReadFile(fileName);
            var existingCount = string.IsNullOrEmpty(fileContent)? 0: Convert.ToInt32(fileContent);
            var newCount = existingCount + 1;
            Database.Replace(fileName, newCount.ToString());
        }
    }
}
