using Microsoft.Extensions.Configuration;

namespace AspnetTemplate.Core.Setup;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddSharedConfig(this IConfigurationBuilder builder)
    {
        var solutionSettings = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "appsettings.json"
        );
        return builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile(solutionSettings, optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();
    }
}
