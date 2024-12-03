using Postech.GroupEight.ContactIntegration.Infra;
using Postech.GroupEight.ContactIntegration.Worker.HealthChecks;
using System.Diagnostics.CodeAnalysis;

namespace Postech.GroupEight.ContactIntegration.Worker.Setup
{
    [ExcludeFromCodeCoverage]
    internal static class HealthCheckSetup
    {
        internal static void AddMongoDbHealthCheck(this IHealthChecksBuilder healthChecks)
        {
            healthChecks.AddCheck<MongoDbHealthCheck>(nameof(MongoDbOptions));
        }

        internal static void AddRabbitMQHealthCheck(this IHealthChecksBuilder healthChecks)
        {
            healthChecks.AddCheck<MassTransitRabbitMqHealthCheck>("RabbitMQ");
        }
    }
}