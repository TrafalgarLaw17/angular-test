using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using System.Linq;
using FRA_Todolist_prj;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog from appsettings.json
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
          .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection", Serilog.Events.LogEventLevel.Fatal);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 



var startup = new Startup(builder.Configuration, args);
startup.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
var serviceProvider = app.Services;

app.MapGet("/endpoints", (HttpContext context, EndpointDataSource endpointDataSource) =>
{
    var endpoints = endpointDataSource.Endpoints
        .OfType<RouteEndpoint>()
        .Select(e => e.RoutePattern.RawText)
        .Where(route => route != null)
        .ToList();

    return Results.Json(endpoints);
});

startup.Configure(app, app.Environment, serviceProvider);
app.Run();
