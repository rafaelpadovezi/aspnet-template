using AspnetTemplate.Api.Configuration;
using AspnetTemplate.Core.Database;
using AspnetTemplate.Core.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedAppSettings();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services
    .AddApiVersioning()
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext"))
);

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<AppDbContext>()
    .AddRabbitMQ(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var descriptions = app.DescribeApiVersions();
    foreach (var description in descriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant()
        );
    }
});

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();

// For testing
namespace AspnetTemplate.Api
{
    public partial class Program { }
}
