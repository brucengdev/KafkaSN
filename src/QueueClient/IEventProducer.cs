namespace QueueClient;

public interface IEventProducer
{
    Task ProduceAsync(string topic, string message);
}