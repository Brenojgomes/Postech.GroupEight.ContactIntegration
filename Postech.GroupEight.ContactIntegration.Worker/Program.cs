using MassTransit;
using Postech.GroupEight.ContactIntegration.Infra;
using Postech.GroupEight.ContactIntegration.Worker;
using Postech.GroupEight.ContactIntegration.Worker.Consumers;
using System.Text.Json;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructure();
        services.AddScoped<ContactIntegrationConsumer>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ContactIntegrationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", host => { });

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

        services.AddMassTransitHostedService(true);

        services.AddHostedService<Worker>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();