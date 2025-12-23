using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace QueueClient;

public class RabbitMQEventProducer : IEventProducer, IDisposable
{
    private IConnection _connection;
    private IChannel _channel;
    public RabbitMQEventProducer(IOptions<RabbitMQProducerConfig> options)
    {
        Setup(options.Value).GetAwaiter().GetResult();
    }

    private async Task Setup(RabbitMQProducerConfig options)
    {
        var factory = new ConnectionFactory { 
            HostName = options.Host, 
            Port = 5672,
            UserName = "myuser",
            Password = "mypassword",
            VirtualHost = "myvhost"
        };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public async Task ProduceAsync(string topic, string message)
    {
        await _channel.QueueDeclareAsync(queue: topic, durable: false, exclusive: false, autoDelete: false,
            arguments: null);
        var body = Encoding.UTF8.GetBytes(message);
        await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: topic, body: body);
        Console.WriteLine($"Delivered '{message}' to '{topic}'");
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}