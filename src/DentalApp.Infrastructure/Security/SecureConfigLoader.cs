using Microsoft.Extensions.Configuration;

namespace DentalApp.Infrastructure.Security;

public static class SecureConfigLoader
{
    public static IConfigurationRoot LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(FileSystem.AppDataDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        return builder.Build();
    }
}