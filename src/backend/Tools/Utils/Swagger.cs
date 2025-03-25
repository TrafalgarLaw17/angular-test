using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Tools.Utils
{
    public static class Swagger
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Todolist Project API",
                    Version = "v1",
                    Description = "Todolist with Basic Auth APIs",
                });

                string? appUrl = configuration["ASPNETCORE_URLS"] ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS");

                GenericLogger.LogAppUrl(appUrl);

                options.AddServer(new OpenApiServer
                {
                    Url = appUrl,
                    Description = "Local Development Server"
                });

                options.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Description = "Enter your username and password"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Basic"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        public static void UseSwaggerDocumentation(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "todo-api";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todolist Project API v1");
            });
        }
    }
}
