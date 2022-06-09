using Common.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SeedWork.Helpers
{
    public static class HealthCheckHelper
    {
       
        public static IHealthChecksBuilder AddDatabaseCheck(this IHealthChecksBuilder hcBuilder, IConfiguration configuration)
        {
            hcBuilder.AddNpgSql(
                new ConnectionStringBuilder(configuration.GetConnectionString("process")).GetPlainString(),
                name: "DB-check",
                tags: new string[] { "database" });

            return hcBuilder;
        }
    }
}
