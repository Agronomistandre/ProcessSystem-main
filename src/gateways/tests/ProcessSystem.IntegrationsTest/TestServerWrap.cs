using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Common.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ProcessSystem.IntegrationsTest
{
    public sealed class TestServerWrap: IDisposable
    {
        public const string TEST_ENVIRONMENT_NAME = "Development";

        public string BasePath { get; }
        
        public string ConfigFileName { get; }

        public IConfigurationRoot Configuration { get; }

        public TestServer TestServer { get; }

        public HttpClient Client { get; set; }

        public TestServerWrap(Type startupType, string configFileName)
        {
            ConfigFileName = configFileName;

            BasePath = Path.GetDirectoryName(Assembly.GetAssembly(startupType).Location);

            Configuration = AppFileConfiguration.GetConfiguration($"{ConfigFileName}", TEST_ENVIRONMENT_NAME, BasePath);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", TEST_ENVIRONMENT_NAME);

            TestServer = new TestServer(new WebHostBuilder()
                .UseContentRoot(BasePath)
                .UseSerilog((hostingContext, loggerConfiguration) => {
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                        .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);

#if DEBUG
                    // Used to filter out potentially bad data due debugging.
                    // Very useful when doing Seq dashboards and want to remove logs under debugging session.
                    loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
                })
                .UseEnvironment(TEST_ENVIRONMENT_NAME)
                .UseConfiguration(Configuration)
                .UseStartup(startupType)
            );

            Client = TestServer.CreateClient();
        }

        public void Dispose()
        {
            TestServer?.Host?.StopAsync();
            TestServer?.Host?.WaitForShutdown();
            TestServer?.Dispose();
        }
    }
}
