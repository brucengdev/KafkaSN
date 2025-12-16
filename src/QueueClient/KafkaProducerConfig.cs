using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QueueClient;

public static class KafkaProducerConfig
{
    public static void AddKafkaProducerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton((_) =>
        {
            var boostrapServers = configuration["Kafka:BootstrapServers"];
            var clientId = configuration["Kafka:ClientId"];
            return new ProducerConfig()
            {
                BootstrapServers = boostrapServers,
                ClientId = clientId
            };
        });
    }
}