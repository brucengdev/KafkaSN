namespace QueueClient;

public interface IEventConsumer
{
    void Subscribe(string[] topics);
    EventData Consume();
}

public static class EventConsumerExtensions {
    public static void Subscribe(this IEventConsumer consumer, string topic)
    {
        consumer.Subscribe([topic]);
    }
}