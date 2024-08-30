using MassTransit;
using Postech.GroupEight.ContactIntegration.Application.Events;
using Postech.GroupEight.ContactIntegration.Application.Services.Interfaces;
using Postech.GroupEight.ContactIntegration.Core.Enumerators;

namespace Postech.GroupEight.ContactIntegration.Worker.Consumers
{
    public class ContactIntegrationConsumer : IConsumer<ContactEvent>
    {
        private readonly ILogger<ContactIntegrationConsumer> _logger;
        private readonly IContactService _contactService;

        public ContactIntegrationConsumer(ILogger<ContactIntegrationConsumer> logger, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        public Task Consume(ConsumeContext<ContactEvent> context)
        {
            Guid identifier = Guid.NewGuid();
            _logger.LogInformation($"Received ContactIntegration message at: {DateTimeOffset.Now} | LogIdentifier: {identifier}");

            switch (context.Message.EventType)
            {
                case EventTypeEnum.Create:
                    _logger.LogInformation($"Creating contact with Id: {context.Message.Id} | LogIdentifier: {identifier}");
                    return _contactService.CreateContactHandlerAsync(context.Message);
                case EventTypeEnum.Update:
                    _logger.LogInformation($"Updating contact with Id: {context.Message.Id} | LogIdentifier: {identifier}");
                    return _contactService.UpdateContactHandlerAsync(context.Message);
                case EventTypeEnum.Delete:
                    _logger.LogInformation($"Deleting contact with Id: {context.Message.Id} | LogIdentifier: {identifier}");
                    return _contactService.DeleteContactHandlerAsync(context.Message.Id, context.Message.AreaCode);
                default:
                    _logger.LogInformation($"Invalid EventType: {context.Message.EventType} | LogIdentifier: {identifier}");
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
