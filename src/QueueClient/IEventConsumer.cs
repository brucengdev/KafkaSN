namespace QueueClient;

public interface IEventConsumer
{
    Task Subscribe(string[] topics);
    Task Consume(Func<EventData, Task> callback, CancellationToken cancellationToken);
}

public static class EventConsumerExtensions {
    public static async Task Subscribe(this IEventConsumer consumer, string topic)
    {
        await consumer.Subscribe([topic]);
    }
}