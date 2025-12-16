namespace QueueClient;

public interface IEventConsumer
{
    void Subscribe(string topic);
    EventData Consume();
}