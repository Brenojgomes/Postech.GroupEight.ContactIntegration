using MassTransit;
using Postech.GroupEight.ContactIntegration.Infra;
using Postech.GroupEight.ContactIntegration.Worker;
using Postech.GroupEight.ContactIntegration.Worker.Consumers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructure();
        services.AddScoped<ContactIntegrationConsumer>();

        services.AddMassTransit(x =>
        {
            var configuration = context.Configuration;
            x.AddConsumer<ContactIntegrationConsumer>();

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
                });

                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
            });
        });

        services.AddHostedService<Worker>();
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