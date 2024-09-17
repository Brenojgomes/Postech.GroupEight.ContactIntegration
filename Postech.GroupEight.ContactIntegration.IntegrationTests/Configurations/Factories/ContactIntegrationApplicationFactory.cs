using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mongo2Go;
using MongoDB.Driver;
using Postech.GroupEight.ContactIntegration.Application.Services;
using Postech.GroupEight.ContactIntegration.Application.Services.Interfaces;
using Postech.GroupEight.ContactIntegration.Core.Entities;
using Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories;
using Postech.GroupEight.ContactIntegration.Infra.Persistence.Repositories;
using Postech.GroupEight.ContactIntegration.IntegrationTests.Configurations.Factories.Extensions;
using Postech.GroupEight.ContactIntegration.Worker.Consumers;
using System.Diagnostics.CodeAnalysis;

namespace Postech.GroupEight.ContactIntegration.IntegrationTests.Configurations.Factories
{
    [ExcludeFromCodeCoverage]
    public class ContactIntegrationApplicationFactory : HostApplicationFactory<Program>, IDisposable
    {
        private readonly string _rabbitMqConnectionString;
        private IConfiguration? _configuration = null;
        private readonly MongoDbRunner _mongoRunner = MongoDbRunner.Start();

        public ContactIntegrationApplicationFactory(string rabbitMqConnectionString)
            : base(builder => ConfigureWebHost(builder, rabbitMqConnectionString))
        {
            _rabbitMqConnectionString = rabbitMqConnectionString;
            _mongoRunner = MongoDbRunner.Start();
        }

        private static void ConfigureWebHost(IWebHostBuilder builder, string rabbitMqConnectionString)
        {
            builder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configurationBuilder.AddJsonFile("appsettings.IntegrationTests.json");
            });

            builder.ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                // Remove existing MassTransit services
                List<ServiceDescriptor> massTransitServiceDescriptors = services.GetMassTransitServiceDescriptors();
                foreach (ServiceDescriptor massTransitServiceDescriptor in massTransitServiceDescriptors)
                {
                    services.Remove(massTransitServiceDescriptor);
                }

                // Configure RabbitMQ
                services.AddMassTransit(m =>
                {
                    m.AddConsumer<ContactIntegrationConsumer>();
                    m.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(rabbitMqConnectionString, h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });

                        cfg.ReceiveEndpoint("contact_integration_queue", e =>
                        {
                            e.ConfigureConsumer<ContactIntegrationConsumer>(context);
                        });
                    });
                });

                // Configure MongoDB with Mongo2Go
                var mongoRunner = MongoDbRunner.Start();
                var mongoDbConnectionString = mongoRunner.ConnectionString;
                var mongoDbDatabaseName = "contacts";
                services.AddSingleton<IMongoClient, MongoClient>(sp =>
                {
                    return new MongoClient(mongoDbConnectionString);
                });
                services.AddSingleton(sp =>
                {
                    var client = sp.GetRequiredService<IMongoClient>();
                    var database = client.GetDatabase(mongoDbDatabaseName);
                    return database;
                });
                services.AddSingleton(sp =>
                {
                    var client = sp.GetRequiredService<IMongoClient>();
                    var database = client.GetDatabase(mongoDbDatabaseName);
                    return database.GetCollection<ContactEntity>("contacts_31");
                });

                // Configure Repositories and Services
                services.AddScoped<IContactRepository, ContactRepository>();
                services.AddScoped<IContactService, ContactService>();

                // Configure Logging
                services.AddLogging(configure => configure.AddConsole());
            });
        }

        public new void Dispose()
        {
            _mongoRunner.Dispose();
            base.Dispose();
        }
    }
}
