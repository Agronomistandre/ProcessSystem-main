using System;
using Common.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ProcessSystem.Middleware
{
    public static class Swagger
    {

        public static void AddSwaggerDoc(this IServiceCollection services)
        {
            SwaggerConfig.UseQueryStringApiVersion("v");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0",
                    new OpenApiInfo
                    {
                        Title = "ProcessSystem",
                        Version = "v1.0",
                        Description =
                            "Тестовое задание",
                    });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(securitySchema, new[] { "Bearer" });
                c.AddSecurityRequirement(securityRequirement);

                c.OperationFilter<SwaggerParameterFilters>();
                c.DocumentFilter<SwaggerVersionMapping>();

                c.CustomSchemaIds(type => type.ToString());
            });
        }
    }
}
