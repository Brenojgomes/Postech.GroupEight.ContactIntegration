using MassTransit;
using Polly.CircuitBreaker;
using Postech.GroupEight.ContactIntegration.Application.Services.Interfaces;
using Postech.GroupEight.ContactIntegration.Worker.PolicyHandler;
using Postech.GroupEight.TechChallenge.ContactManagement.Events;
using Prometheus;

namespace Postech.GroupEight.ContactIntegration.Worker.Consumers
{
    public class DeleteIntegrationConsumer : IConsumer<DeleteIntegrationModel>
    {
        private readonly ILogger<DeleteIntegrationConsumer> _logger;
        private readonly IContactService _contactService;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        private static readonly Counter DeleteEventCounter = Metrics.CreateCounter("contact_delete_events_total", "Total number of contact delete events");


        public DeleteIntegrationConsumer(ILogger<DeleteIntegrationConsumer> logger, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
            _circuitBreakerPolicy = PolicyHandlerConfig.GetCircuitBreakerPolicy(logger);
        }

        /// <summary>
        /// Consumes the delete integration event.
        /// </summary>
        /// <param name="context">The consume context.</param>
        public async Task Consume(ConsumeContext<DeleteIntegrationModel> context)
        {
            try
            {
                await _circuitBreakerPolicy.ExecuteAsync(async () =>
                {              
                    DeleteEventCounter.Inc();
                    await _contactService.DeleteContactHandlerAsync(context.Message.Id);
                    _logger.LogInformation($"Contact deleted: {context.Message.Id}");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing delete event for contact ID: {context.Message.Id}");
                throw;
            }
        }
    }
}
