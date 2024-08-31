using MassTransit;
using Postech.GroupEight.ContactIntegration.Application.Events;
using Postech.GroupEight.ContactIntegration.Core.Enumerators;

namespace Postech.GroupEight.ContactIntegration.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBus _bus;

        public Worker(ILogger<Worker> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            try
            {
                // Criando um evento para testes
                var contactEvent = new ContactEvent
                {
                    Id = Guid.Parse("7a0f9dee-976a-4623-a55b-30b0e353a8ba"),
                    AreaCode = "11",
                    Number = "97458236",
                    FirstName = "Teste",
                    LastName = "lastName",
                    Email = "teste@teste.com",
                    Active = true,
                    EventType = EventTypeEnum.Create
                };

                // Publicando o evento
                await _bus.Publish(contactEvent, stoppingToken);

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    }
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }          
        }
    }
}
