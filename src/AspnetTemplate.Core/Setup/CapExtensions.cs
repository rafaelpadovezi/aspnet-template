using AspnetTemplate.Core.Database;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspnetTemplate.Core.Setup;

public static class CapExtensions
{
    public static CapBuilder AddCap(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddCap(x =>
        {
            x.UseEntityFramework<AppDbContext>();
            x.UseRabbitMQ(o =>
            {
                o.HostName =
                    configuration.GetValue<string>("RabbitMQ:HostName")
                    ?? throw new InvalidOperationException();
                o.Port = configuration.GetValue<int>("RabbitMQ:Port");
                o.VirtualHost =
                    configuration.GetValue<string>("RabbitMQ:VirtualHost")
                    ?? throw new InvalidOperationException();
                o.ExchangeName =
                    configuration.GetValue<string>("RabbitMQ:ExchangeName")
                    ?? throw new InvalidOperationException();
                o.UserName =
                    configuration.GetValue<string>("RabbitMQ:UserName")
                    ?? throw new InvalidOperationException();
                o.Password =
                    configuration.GetValue<string>("RabbitMQ:Password")
                    ?? throw new InvalidOperationException();
                //set optionally the prefetch count for messages (how many will enqueue in memory at a time before ack)
                o.BasicQosOptions = new RabbitMQOptions.BasicQos(
                    configuration.GetValue<ushort>("RabbitMQ:BasicQos")
                );
            });
        });
    }
}
