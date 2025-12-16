using Confluent.Kafka;

namespace QueueClient;

public class KafkaProducer
{
    private readonly IProducer<Null, string> _producer;
    public KafkaProducer(ProducerConfig config)
    {
        // The ProducerBuilder is used to create the producer instance
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, string message)
    {
        var dr = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
    }

    // Ensure Flush and Dispose are called when the application shuts down
    public void Dispose()
    {
        _producer.Flush();
        _producer.Dispose();
    }
}
