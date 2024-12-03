using MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;

namespace Postech.GroupEight.ContactIntegration.Worker.HealthChecks
{
    [ExcludeFromCodeCoverage]
    internal class MassTransitRabbitMqHealthCheck(IBus bus) : IHealthCheck
    {
        private readonly IBus _bus = bus;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                ISendEndpoint sendEndpoint = await _bus.GetSendEndpoint(new Uri("queue:health-check"));
                await sendEndpoint.Send(new HealthCheckMessage(), cancellationToken);
                return HealthCheckResult.Healthy("RabbitMQ está disponível através do MassTransit.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("RabbitMQ indisponível através do MassTransit.", ex);
            }
        }

        [ExcludeFromCodeCoverage]
        private class HealthCheckMessage
        {
            public string Text { get; set; } = "Health Check";
        }
    }
}