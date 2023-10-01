using AspnetTemplate.Core.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Respawn;

namespace AspnetTemplate.Tests.Support;

public class TransactionalAppFixture : WebApplicationFactory<Api.Program>
{
    public AppDbContext DbContext { get; }

    public TransactionalAppFixture()
    {
        var scope = Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        ResetDb().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        builder.ConfigureServices(services =>
        {
            // Override AppDbContext
            services.RemoveAll<AppDbContext>();
            services.RemoveAll<DbContextOptions<AppDbContext>>();

            var connectionStringBuilder = new SqlConnectionStringBuilder(
                configuration.GetConnectionString("AppDbContext")
            )
            {
                InitialCatalog = "TestDb"
            };

            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(connectionStringBuilder.ConnectionString)
            );
        });
    }

    public async Task ResetDb()
    {
        var connectionString = DbContext.Database.GetConnectionString();
        if (connectionString is null)
            return;
        var respawner = await Respawner.CreateAsync(connectionString);
        await respawner.ResetAsync(connectionString);
    }
}
