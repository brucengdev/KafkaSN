using Confluent.Kafka;

namespace QueueClient;

public class KafkaEventConsumer: IEventConsumer
{
    private IConsumer<Ignore, string> _consumer;
    public KafkaEventConsumer(ConsumerConfig config)
    {
        var consumerBuilder = new ConsumerBuilder<Ignore, string>(config);
        _consumer = consumerBuilder.Build();
    }

    public async Task Subscribe(string[] topics)
    {
        _consumer.Subscribe(topics);
    }

    public async Task<EventData> Consume()
    {
        var result = _consumer.Consume();
        Console.WriteLine($"Received event {result?.Message?.Value??""} from topic {result?.Topic??""}");
        return new EventData()
        {
            Topic = result?.Topic,
            Message = result?.Message?.Value
        };
    }
}