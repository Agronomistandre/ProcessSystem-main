using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Common.DB;
using Common.Extensions;
using Common.Helpers;
using Microsoft.EntityFrameworkCore;
using ProcessSystem.DB;
using ProcessSystem.Middleware;
using ProcessSystem.Token;
using Prometheus;
using ProcessSystem.Implementation;

namespace ProcessSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            using var collector = PrometheusCollectorHelper.CreateCollector();

            services.AddControllers();
            services.AddConfigurationOptions(Configuration);

            services.AddSwaggerDoc();
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddMvc().AddNewtonsoftJson();

            services.AddTransient<IToken, TokenImpl>();

            services.AddDbContext<ProcessContext>(
                options =>
                {
                    options.UseNpgsql(
                        new ConnectionStringBuilder(Configuration.GetConnectionString("process")).GetPlainString(),
                        o =>
                            o.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorCodesToAdd: null));

                },
                ServiceLifetime.Scoped);

            services.AddTransient<IRegisterRepository, RegisterRepository>();
            services.AddDatabaseDeveloperPageExceptionFilter();

            Authentication.AddAuthentication(services);

            services.AddHealthChecks(Configuration, _env.EnvironmentName);

            services.AddEventHubVersion();

            // Registers required services for health checks
            services.AddHealthChecksUI().AddInMemoryStorage();
            services.AddHttpClient<IStartProcessService, StartProcessService>();
            services.Configure<NameParam>(options => Configuration.GetSection("NameParam").Bind(options));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ValidateConfigurationOptions();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "ProcessSystem v1.0"));


            app.UseExceptionHandler("/error");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapStandardHealthChecks();
            });
            app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");

            app.UseMetricServer();
            app.UseDeveloperExceptionPage();
        }
    }
}
