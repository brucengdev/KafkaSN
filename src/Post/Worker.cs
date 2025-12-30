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
        logger.LogInformation("Waiting for events from topic 'createPost`");
        await consumer.Consume(async createPostEvent => {
            var post = createPostEvent.Parse<Post>();

            Database.Append(Store.POSTS, post.Username + ":\n\t" + post.Content + "\n");
            await producer.ProduceAsync("postCreated", post);
        }, stoppingToken);
    }
}
