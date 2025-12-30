using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QueueClient;

public class RabbitMQConfig
{
    public string Host {get; set; }
    public int Port { get; set; }
    public string VirtualHost { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }
}

public static class ExtensionsForRabbitMQConfig
{
    public static void AddRabbitMQConfig(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<RabbitMQConfig>(config.GetSection("RabbitMQ"));
    }
}