using Confluent.Kafka;

namespace QueueClient;

public class KafkaEventConsumer: IEventConsumer
{
    private IConsumer<Null, string> _consumer;
    public KafkaEventConsumer(ConsumerConfig config)
    {
        var consumerBuilder = new ConsumerBuilder<Null, string>(config);
        _consumer = consumerBuilder.Build();
    }

    public void Subscribe(string topic)
    {
        _consumer.Subscribe(topic);
    }

    public EventData Consume()
    {
        var result = _consumer.Consume();
        return new EventData
        {
            Topic = result?.Topic,
            Message = result?.Message?.Value
        };
    }
}