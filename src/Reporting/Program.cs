using  Reporting;
using Confluent.Kafka;
using QueueClient;
using Microsoft.Extensions.Options;

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
        consumer.Subscribe(["postCreated"]).GetAwaiter().GetResult();

        return consumer;
    });
} else {
    builder.Services.AddKafkaConsumerConfig(builder.Configuration);
    builder.Services.AddSingleton<IEventConsumer>(services =>
    {
        var config = services.GetRequiredService<ConsumerConfig>();
        var consumer = new KafkaEventConsumer(config);
        consumer.Subscribe(["postCreated"]).GetAwaiter().GetResult();

        return consumer;
    });
}

var host = builder.Build();
host.Run();
