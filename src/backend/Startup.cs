using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentMigrator.Runner;
using FRA_Todolist_prj.Models.Contexts.Services;
using FRA_Todolist_prj.Models.Contexts;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj
{
    public class Startup
    {
        private readonly string[] _args;

        public Startup(IConfiguration configuration, string[]? args = null)
        {
            _args = args ?? Array.Empty<string>();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? throw new Exception("Missing environment variable: DB_HOST");
            string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? throw new Exception("Missing environment variable: DB_PORT");
            string dbName = Environment.GetEnvironmentVariable("DB_DATABASE") ?? throw new Exception("Missing environment variable: DB_DATABASE");
            string dbUser = Environment.GetEnvironmentVariable("DB_USERNAME") ?? throw new Exception("Missing environment variable: DB_USERNAME");
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new Exception("Missing environment variable: DB_PASSWORD");

            string connectionString = $"Server={dbHost};Port={dbPort};Database={dbName};User={dbUser};Password={dbPassword};";

            // DB Context
            services.AddScoped<DBContext>(_ => new DBContext(connectionString));

            // Controllers
            services.AddControllers();

            // Authentication & Authorization
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuth>("BasicAuthentication", null);
            services.AddAuthorization();

            // BCrypt for hashing
            services.AddSingleton<BCrypt.Net.BCrypt>();

            // MigratorConfig (fluent migrator)
             services.AddFluentMigratorCore()
            .ConfigureRunner(configure => configure
                .AddMySql8()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()) 
            .AddLogging(config => config.AddFluentMigratorConsole());

            // SwaggerConfig via Utils
            services.AddSwaggerDocumentation(configuration);

            services.AddCors(options =>
             {
                options.AddPolicy("AllowAngular",
                    builder => builder.WithOrigins("http://localhost:4200") // Allow Angular
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            services.AddControllers();
        }


        public void Configure(WebApplication app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }

            // Run migrations
            Migrator.RunMigrations(serviceProvider, _args);
            
            app.UseCors("AllowAngular");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
