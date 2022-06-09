using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using SeedWork.Helpers;

namespace ProcessSystem.Middleware
{
    public static class HealthChecks
    {
        public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration, string envName)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());


            hcBuilder.AddDatabaseCheck(configuration);

            hcBuilder.ForwardToPrometheus();
        }
    }
}
