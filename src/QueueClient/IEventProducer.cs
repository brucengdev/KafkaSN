using System.Text.Json;

namespace QueueClient;

public interface IEventProducer
{
    Task ProduceAsync(string topic, string message);
}

public static class EventProducerExtensions
{
    public static Task ProduceAsync<T>(this IEventProducer producer, string topic, T eventData)
    {
        return producer.ProduceAsync(topic, JsonSerializer.Serialize<T>(eventData));
    }
}