namespace Post;

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
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Waiting for events from topic 'createPost`");
            var createPostEvent = consumer.Consume();

            var post = createPostEvent.Parse<Post>();

            Database.Append(Store.POSTS, post.Username + ":\n\t" + post.Content + "\n");
            await producer.ProduceAsync("postCreated", post);
        }
    }
}
