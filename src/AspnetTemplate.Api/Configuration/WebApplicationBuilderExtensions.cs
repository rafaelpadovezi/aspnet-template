using AspnetTemplate.Core.Setup;

namespace AspnetTemplate.Api.Configuration;

public static class WebApplicationBuilderExtensions
{
    public static void AddSharedAppSettings(this WebApplicationBuilder builder)
    {
        var configuration = new ConfigurationBuilder().AddSharedConfig().Build();
        builder.Configuration.AddConfiguration(configuration);
    }
}
