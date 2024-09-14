using System.Diagnostics.CodeAnalysis;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
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

namespace Postech.GroupEight.ContactIntegration.IntegrationTests.Configurations.Factories
{
    [ExcludeFromCodeCoverage]
    public class ContactIntegrationApplicationFactory(string rabbitMqConnectionString) : WebApplicationFactory<Program>, IDisposable
    {
        private readonly string _rabbitMqConnectionString = rabbitMqConnectionString;
        private IConfiguration? _configuration = null;
        private readonly MongoDbRunner _mongoRunner = MongoDbRunner.Start();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configurationBuilder.AddJsonFile("appsettings.IntegrationTests.json");
                _configuration = configurationBuilder.Build();
            });

            builder.ConfigureServices(services =>
            {
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
                        cfg.Host(_rabbitMqConnectionString, h =>
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
                var mongoDbConnectionString = _mongoRunner.ConnectionString;
                var mongoDbDatabaseName = "ContactIntegrationTestDb";
                services.AddSingleton<IMongoClient, MongoClient>(sp =>
                {
                    return new MongoClient(mongoDbConnectionString);
                });
                services.AddSingleton(sp =>
                {
                    var client = sp.GetRequiredService<IMongoClient>();
                    var database = client.GetDatabase(mongoDbDatabaseName);
                    return database.GetCollection<ContactEntity>("Contacts");
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
