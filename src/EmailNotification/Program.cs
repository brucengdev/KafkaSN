using Confluent.Kafka;
using  EmailNotification;
using Microsoft.Extensions.Options;
using QueueClient;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();


var queueToUse = builder.Configuration["QUEUE"];
if(queueToUse == "rabbitmq")
{
    builder.Services.AddRabbitMQConfig(builder.Configuration);
    builder.Services.AddSingleton<IEventConsumer>(services =>
    {
        var config = services.GetRequiredService<IOptions<RabbitMQConfig>>();
        var consumer = new RabbitMQEventConsumer(config);
        consumer.Subscribe(["accountCreated", "postCreated"]).GetAwaiter().GetResult();

        return consumer;
    });
} else {
    builder.Services.AddKafkaConsumerConfig(builder.Configuration);
    builder.Services.AddSingleton<IEventConsumer>(services =>
    {
        var config = services.GetRequiredService<ConsumerConfig>();
        var consumer = new KafkaEventConsumer(config);
        consumer.Subscribe(["accountCreated", "postCreated"]).GetAwaiter().GetResult();

        return consumer;
    });
}

var host = builder.Build();
host.Run();
