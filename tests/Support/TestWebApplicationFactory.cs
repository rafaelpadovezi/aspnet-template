using AspnetTemplate.Core.Database;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspnetTemplate.Tests.Support;

public class TestWebApplicationFactory : WebApplicationFactory<Api.Program>
{
    private readonly IDbContextTransaction _transation;
    public AppDbContext DbContext { get; }

    public TestWebApplicationFactory()
    {
        DbContext = Services.GetRequiredService<AppDbContext>();
        _transation = DbContext.Database.BeginTransaction();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        builder.ConfigureServices(services =>
        {
            // Override ApplicationContext
            services.RemoveAll<AppDbContext>();
            services.RemoveAll<DbContextOptions<AppDbContext>>();

            var builder = new SqlConnectionStringBuilder(
                configuration.GetConnectionString("AppDbContext")
            )
            {
                InitialCatalog = "TestDb"
            };

            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(builder.ConnectionString),
                ServiceLifetime.Singleton
            );
        });
    }

    protected override void Dispose(bool disposing)
    {
        _transation.Rollback();
    }
}
