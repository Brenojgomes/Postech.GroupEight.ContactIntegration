﻿using MassTransit;
using Polly.CircuitBreaker;
using Postech.GroupEight.TechChallenge.ContactManagement.Events;
using Postech.GroupEight.ContactIntegration.Application.Services.Interfaces;
using Postech.GroupEight.ContactIntegration.Core.Enumerators;
using Postech.GroupEight.ContactIntegration.Worker.PolicyHandler;
using Prometheus;

namespace Postech.GroupEight.ContactIntegration.Worker.Consumers
{
    /// <summary>
    /// Represents a consumer for processing contact integration events.
    /// </summary>
    public class ContactIntegrationConsumer : IConsumer<ContactIntegrationModel>
    {
        private readonly ILogger<ContactIntegrationConsumer> _logger;
        private readonly IContactService _contactService;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        // Define Prometheus counters for each event type
        private static readonly Counter CreateEventCounter = Metrics.CreateCounter("contact_create_events_total", "Total number of contact create events");
        private static readonly Counter UpdateEventCounter = Metrics.CreateCounter("contact_update_events_total", "Total number of contact update events");

        public ContactIntegrationConsumer(ILogger<ContactIntegrationConsumer> logger, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
            _circuitBreakerPolicy = PolicyHandlerConfig.GetCircuitBreakerPolicy(logger);
        }

        /// <summary>
        /// Consumes the contact integration event.
        /// </summary>
        /// <param name="context">The consume context.</param>
        public async Task Consume(ConsumeContext<ContactIntegrationModel> context)
        {
            try
            {
                await _circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    switch (context.Message.EventType)
                    {
                        case EventTypeEnum.Create:
                            CreateEventCounter.Inc();
                            await _contactService.CreateContactHandlerAsync(context.Message);
                            _logger.LogInformation($"Contact created: {context.Message.Id}");
                            break;
                        case EventTypeEnum.Update:          
                            UpdateEventCounter.Inc();
                            await _contactService.UpdateContactHandlerAsync(context.Message);
                            _logger.LogInformation($"Contact updated: {context.Message.Id}");
                            break;
                        default:
                            _logger.LogInformation($"Invalid EventType: {context.Message.EventType}");
                            break;
                    }
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error processing {context.Message.EventType} event for contact ID: {context.Message.Id}");
                throw;
            }
        }
    }
}
