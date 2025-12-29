using Account;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using QueueClient;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// builder.Services.AddKafkaConsumerConfig(builder.Configuration);
// builder.Services.AddSingleton<IEventConsumer>(services =>
// {
//     var config = services.GetRequiredService<ConsumerConfig>();
//     var consumer = new KafkaEventConsumer(config);
//     consumer.Subscribe("createAccount");

//     return consumer;
// });

// builder.Services.AddKafkaProducerConfig(builder.Configuration);
// builder.Services.AddSingleton<IEventProducer, KafkaProducer>();

builder.Services.AddRabbitMQConfig(builder.Configuration);
builder.Services.AddSingleton<IEventConsumer>(services =>
{
    var config = services.GetRequiredService<IOptions<RabbitMQConfig>>();
    var consumer = new RabbitMQEventConsumer(config);
    consumer.Subscribe("createAccount").GetAwaiter().GetResult();

    return consumer;
});

builder.Services.AddSingleton<IEventProducer, RabbitMQEventProducer>();

var host = builder.Build();
host.Run();
