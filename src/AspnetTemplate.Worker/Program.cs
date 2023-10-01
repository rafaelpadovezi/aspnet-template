using AspnetTemplate.Core.Database;
using AspnetTemplate.Core.Setup;
using AspnetTemplate.Worker;
using AspnetTemplate.Worker.Configuration;
using AspnetTemplate.Worker.ProductCreated;
using Microsoft.EntityFrameworkCore;
using Ziggurat;
using Ziggurat.CapAdapter;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.Sources.Clear();
        builder.AddSharedConfig();
    })
    .ConfigureServices(
        (hostContext, services) =>
        {
            var configuration = hostContext.Configuration;
            if (args[0] == "product-created-consumer")
            {
                services
                    .AddScoped<ProductCreatedConsumer>()
                    .AddConsumerService<ProductCreatedMessage, ProductCreatedService>(options =>
                    {
                        options.Use<LoggingMiddleware<ProductCreatedMessage>>();
                        options.UseEntityFrameworkIdempotency<
                            ProductCreatedMessage,
                            AppDbContext
                        >();
                    });
            }
            else
            {
                throw new Exception();
            }
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("AppDbContext"))
            );
            services.AddCap(configuration).AddSubscribeFilter<BootstrapFilter>();
            services.AddHostedService<Worker>();
        }
    )
    .Build();

host.Run();
