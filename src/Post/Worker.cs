namespace Post;

using QueueClient;
using Models;
using Database;

public class Worker(ILogger<Worker> logger, IEventConsumer consumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Waiting for events from topic 'createPost`");
            var createPostEvent = consumer.Consume();

            logger.LogInformation("event: " + createPostEvent.Message);

            var post = createPostEvent.Parse<Post>();

            Database.Append(Store.POSTS, post.Username + " " + post.Content);
        }
    }
}
