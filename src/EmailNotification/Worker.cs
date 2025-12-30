namespace EmailNotification;

using QueueClient;
using Models;
using Database;

public class Worker(ILogger<Worker> logger, 
    IEventConsumer consumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        logger.LogInformation("Waiting for events from topic 'accountCreated` and `postCreated`");
        await consumer.Consume(async receivedEvent => {
            switch(receivedEvent.Topic)
            {
                case "accountCreated": SendAccountCreationEmail(receivedEvent.Parse<Account>()); break;
                case "postCreated": SendPostCreationEmail(receivedEvent.Parse<Post>());break;
            }
        }, stoppingToken);
    }

    private void SendAccountCreationEmail(Account account)
    {
        Database.Append($"emails/accountCreated-{account.Username}-{TimeStamp()}.txt", $"Account {account.Username} was created");
    }

    private void SendPostCreationEmail(Post post)
    {
        Database.Append($"emails/post-{post.Username}-{TimeStamp()}.txt", $"User {post.Username} posted a message: \n{post.Content}");
    }

    private string TimeStamp()
    {
        return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
    }
}
