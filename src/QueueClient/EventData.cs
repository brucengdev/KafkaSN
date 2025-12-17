using System.Text.Json;

namespace QueueClient;

public class EventData
{
    public string Topic { get; set; }
    public string? Message { get; set; }

    public T? Parse<T>() => Message == null? default(T): JsonSerializer.Deserialize<T>(Message);
}