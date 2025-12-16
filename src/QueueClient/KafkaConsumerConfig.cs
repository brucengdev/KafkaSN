using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QueueClient;

public static class KafkaConsumerConfig
{
    public static void AddKafkaConsumerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton((_) =>
        {
            var boostrapServers = configuration["Kafka:BootstrapServers"];
            var groupId = configuration["Kafka:GroupId"];
            return new ConsumerConfig()
            {
                BootstrapServers = boostrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };
        });
    }
}