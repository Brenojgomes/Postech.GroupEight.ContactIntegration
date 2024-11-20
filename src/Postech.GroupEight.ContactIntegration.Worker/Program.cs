using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Postech.GroupEight.ContactIntegration.Infra;
using Postech.GroupEight.ContactIntegration.Worker;
using Postech.GroupEight.ContactIntegration.Worker.Consumers;
using Postech.GroupEight.ContactIntegration.Worker.Setup;
using Prometheus;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        IHostEnvironment env = context.HostingEnvironment;
        config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructure();
        services.AddScoped<DeleteIntegrationConsumer>();
        services.AddScoped<ContactIntegrationConsumer>();

        services.AddMassTransit(x =>
        {
            IConfiguration configuration = context.Configuration;
            x.AddConsumer<ContactIntegrationConsumer>();
            x.AddConsumer<DeleteIntegrationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("RabbitMq"), host =>
                {
                    host.Username(configuration.GetConnectionString("RabbitMqUser"));
                    host.Password(configuration.GetConnectionString("RabbitMqPassword"));
                });

                cfg.ConfigureJsonSerializerOptions(options =>
                {
                    options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    return options;
                });

                cfg.ReceiveEndpoint("contact.integration", e =>
                {
                    e.ConfigureConsumer<ContactIntegrationConsumer>(context);
                    e.ConfigureConsumer<DeleteIntegrationConsumer>(context);
                });

                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
            });
        });

        services.AddHealthChecks().AddMongoDbHealthCheck();
        services.AddHealthChecks().AddRabbitMQHealthCheck();

        services.AddHostedService<Worker>();

        KestrelMetricServer metricsServer = new(port: 5678);
        metricsServer.Start();
    })
     .ConfigureWebHostDefaults(webBuilder =>
     {
         webBuilder.Configure(app =>
         {
             app.UseRouting();

             // Health check server in 5679 port
             app.UseEndpoints(endpoints =>
             {
                 endpoints.MapHealthChecks("/health");
                 endpoints.MapHealthChecks("/ready", new HealthCheckOptions
                 {
                     Predicate = healthCheck => healthCheck.Tags.Contains("ready")
                 });
             });
         })
         .UseUrls("http://0.0.0.0:5679");
     })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();

[ExcludeFromCodeCoverage]
public partial class Program
{
    protected Program() { }
}