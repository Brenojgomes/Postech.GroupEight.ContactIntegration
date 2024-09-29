using MassTransit;
using Postech.GroupEight.ContactIntegration.Infra;
using Postech.GroupEight.ContactIntegration.Worker;
using Postech.GroupEight.ContactIntegration.Worker.Consumers;
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

        services.AddHostedService<Worker>();

        KestrelMetricServer metricsServer = new(port: 5678);
        metricsServer.Start();
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