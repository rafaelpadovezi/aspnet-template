namespace AspnetTemplate.Api.Configuration;

public static class HealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddRabbitMQ(
        this IHealthChecksBuilder healthChecksBuilder,
        IConfiguration configuration
    )
    {
        var userName = configuration.GetValue<string>("RabbitMQ:UserName");
        var password = configuration.GetValue<string>("RabbitMQ:Password");
        var port = configuration.GetValue<int>("RabbitMQ:Port");
        var hostName = configuration.GetValue<string>("RabbitMQ:HostName");
        var virtualHost = configuration.GetValue<string>("RabbitMQ:VirtualHost");
        var rabbitConnection = $"amqp://{userName}:{password}@{hostName}:{port}/{virtualHost}";

        healthChecksBuilder.AddRabbitMQ(rabbitConnectionString: rabbitConnection);

        return healthChecksBuilder;
    }
}
