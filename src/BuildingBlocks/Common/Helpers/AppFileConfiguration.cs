using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Common.Helpers
{
    public static class AppFileConfiguration
    {
        public static IConfigurationRoot GetConfiguration(string configFileName, string envVariable, string basePath = null) =>
            new ConfigurationBuilder()
                .AddJsonFile($"{configFileName}.json")
                .AddJsonFile($"{configFileName}.{envVariable}.json", optional: true)
                .SetBasePath(
                    string.IsNullOrWhiteSpace(basePath) ?
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) :
                    basePath
                    )
                .AddEnvironmentVariables()
                .Build();

    }
}