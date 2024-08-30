using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Postech.GroupEight.ContactIntegration.Application.Services;
using Postech.GroupEight.ContactIntegration.Application.Services.Interfaces;
using Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories;
using Postech.GroupEight.ContactIntegration.Infra.Persistence.Repositories;

namespace Postech.GroupEight.ContactIntegration.Infra
{
    public static class InfrastuctureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {     
            services.AddMongo();
            services.AddRepositories();
            return services;
        }

        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbOptions>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                var options = new MongoDbOptions();
                configuration.GetSection("Mongo").Bind(options);
                return options;
            });

            services.AddSingleton<IMongoClient>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                var options = sp.GetService<MongoDbOptions>();
                var client = new MongoClient(options.ConnectionString);
                return client;
            });

            services.AddTransient(sp =>
            {
                BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
                var client = sp.GetService<IMongoClient>();
                var options = sp.GetService<MongoDbOptions>();
                return client.GetDatabase(options.Database);
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IContactRepository, ContactRepository>();
            services.AddSingleton<IContactService, ContactService>();
            return services;
        }   
    }
}