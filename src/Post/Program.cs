using Post;
using Confluent.Kafka;
using QueueClient;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddKafkaConsumerConfig(builder.Configuration);
builder.Services.AddSingleton<IEventConsumer>(services =>
{
    var config = services.GetRequiredService<ConsumerConfig>();
    var consumer = new KafkaEventConsumer(config);
    consumer.Subscribe("createPost");

    return consumer;
});

var host = builder.Build();
host.Run();
