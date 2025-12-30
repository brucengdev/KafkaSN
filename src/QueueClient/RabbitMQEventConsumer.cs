using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueueClient;

public class RabbitMQEventConsumer : IEventConsumer, IDisposable
{
    private IConnection _connection;
    private IChannel _channel;

    //map topic to queue names
    private Dictionary<string, string> _topicToQueue = new();
    public RabbitMQEventConsumer(IOptions<RabbitMQConfig> options)
    {
        Setup(options.Value).GetAwaiter().GetResult();
    }

    private async Task Setup(RabbitMQConfig options)
    {
        var factory = new ConnectionFactory { 
            HostName = options.Host, 
            Port = options.Port,
            UserName = options.Username,
            Password = options.Password,
            VirtualHost = options.VirtualHost
        };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    
    public async Task Subscribe(string[] topics)
    {
        foreach(var topic in topics) {
            await _channel.ExchangeDeclareAsync(exchange: topic, type: ExchangeType.Fanout);

            QueueDeclareOk queueDeclareResult = await _channel.QueueDeclareAsync();
            string queueName = queueDeclareResult.QueueName;
            await _channel.QueueBindAsync(queue: queueName, exchange: topic, routingKey: string.Empty);

            _topicToQueue[topic] = queueName;
        }
    }

    public async Task Consume(Func<EventData, Task> callback, CancellationToken cancellationToken)
    {
        await Task.Yield();
        var topics = _topicToQueue.Keys;
        var tasks = new List<Task>();
        Console.WriteLine($"Topics = [{string.Join(",", topics)}]");
        foreach(var topic in topics) {
            var queue = _topicToQueue[topic];
        Console.WriteLine($"Topic={topic}, Queue={queue}");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] {message}");
                await callback(new EventData
                {
                    Topic = topic,
                    Message = message
                });
            };
            var consumeTask = _channel.BasicConsumeAsync(queue, autoAck: true, consumer, cancellationToken);
            tasks.Add(consumeTask);
        }
        await Task.WhenAll(tasks);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}