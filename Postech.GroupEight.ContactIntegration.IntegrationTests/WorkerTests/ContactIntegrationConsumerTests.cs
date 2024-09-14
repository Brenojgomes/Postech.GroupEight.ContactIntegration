using MassTransit;
using MongoDB.Driver;
using Postech.GroupEight.ContactIntegration.Application.Events;
using Postech.GroupEight.ContactIntegration.Core.Entities;
using Postech.GroupEight.ContactIntegration.Core.Enumerators;
using Postech.GroupEight.ContactIntegration.IntegrationTests.Configurations.Base;
using Postech.GroupEight.ContactIntegration.IntegrationTests.Fixtures;


namespace Postech.GroupEight.ContactIntegration.IntegrationTests.WorkerTests
{
    [Collection("Integration Tests")]
    public class ContactIntegrationConsumerTests : IntegrationTestBase
    {
        private readonly IBusControl _busControl;
        private readonly IMongoCollection<ContactEntity> _contactCollection;

        public ContactIntegrationConsumerTests(IntegrationTestFixture fixture) : base(fixture)
        {
            _busControl = GetService<IBusControl>();
            _contactCollection = GetService<IMongoCollection<ContactEntity>>();
        }

        public async Task Should_Create_Contact_When_ContactEvent_Is_Received()
        {
            // Arrange
            var contactEvent = new ContactEvent
            {
                Id = Guid.NewGuid(),
                EventType = EventTypeEnum.Create,
                AreaCode = "31",
                Number = "45678907",
                FirstName = "Breno",
                LastName = "Gomes",
                Email = "teste@teste.com",
                Active = true
            };

            // Act
            await _busControl.Publish(contactEvent);

            await Task.Delay(5000);

            // Assert
            var contact = await _contactCollection.Find(c => c.Id == contactEvent.Id).FirstOrDefaultAsync();
            Assert.NotNull(contact);
            Assert.Equal(contactEvent.Id, contact.Id);
            Assert.Equal(contactEvent.FirstName, contact.FirstName);
            Assert.Equal(contactEvent.LastName, contact.LastName);
            Assert.Equal(contactEvent.Email, contact.Email);
        }
    }
}
