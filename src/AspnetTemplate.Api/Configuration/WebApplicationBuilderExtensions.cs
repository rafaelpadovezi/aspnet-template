namespace AspnetTemplate.Api.Configuration;

public static class WebApplicationBuilderExtensions
{
    public static void AddSharedAppSettings(this WebApplicationBuilder builder)
    {
        var solutionSettings = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "appsettings.json"
        );
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile(solutionSettings, optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
        builder.Configuration.AddConfiguration(configuration);
    }
}
