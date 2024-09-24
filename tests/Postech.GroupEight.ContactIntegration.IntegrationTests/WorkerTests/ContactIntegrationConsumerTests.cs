using MassTransit;
using MongoDB.Driver;
using Postech.GroupEight.TechChallenge.ContactManagement.Events;
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

        [Fact]
        public async Task Should_Create_Contact_When_ContactEvent_Is_Received()
        {
            // Arrange
            var contactEvent = new ContactIntegrationModel
            {
                Id = Guid.NewGuid(),
                EventType = EventTypeEnum.Create,
                AreaCode = "31",
                PhoneNumber = "45678907",
                FirstName = "Breno",
                LastName = "Gomes",
                Email = "teste@teste.com",
                Active = true
            };

            // Act
            await _busControl.Publish(contactEvent);

            await Task.Delay(1000);

            // Assert
            var contact = await _contactCollection.Find(c => c.Id == contactEvent.Id).FirstOrDefaultAsync();
            Assert.NotNull(contact);
            Assert.Equal(contactEvent.Id, contact.Id);
            Assert.Equal(contactEvent.FirstName, contact.FirstName);
            Assert.Equal(contactEvent.LastName, contact.LastName);
            Assert.Equal(contactEvent.Email, contact.Email);
        }

        [Fact]
        public async Task Should_Update_Contact_When_ContactEvent_Is_Received()
        {
            // Arrange
            var contactId = Guid.NewGuid();
            var initialContact = new ContactEntity
            {
                Id = contactId,
                AreaCode = "31",
                Number = "45678907",
                FirstName = "Breno",
                LastName = "Gomes",
                Email = "teste@teste.com",
                Active = true
            };
            await _contactCollection.InsertOneAsync(initialContact);

            var updateEvent = new ContactIntegrationModel
            {
                Id = contactId,
                EventType = EventTypeEnum.Update,
                AreaCode = "31",
                PhoneNumber = "45678907",
                FirstName = "Bruno",
                LastName = "Silva",
                Email = "updated@teste.com",
                Active = true
            };

            // Act
            await _busControl.Publish(updateEvent);

            await Task.Delay(2000);

            // Assert
            var updatedContact = await _contactCollection.Find(c => c.Id == updateEvent.Id).FirstOrDefaultAsync();
            Assert.NotNull(updatedContact);
            Assert.Equal(updateEvent.Id, updatedContact.Id);
            Assert.Equal(updateEvent.FirstName, updatedContact.FirstName);
            Assert.Equal(updateEvent.LastName, updatedContact.LastName);
            Assert.Equal(updateEvent.Email, updatedContact.Email);
        }

        [Fact]
        public async Task Should_Delete_Contact_When_ContactEvent_Is_Received()
        {
            // Arrange
            var contactId = Guid.NewGuid();
            var initialContact = new ContactEntity
            {
                Id = contactId,
                AreaCode = "31",
                Number = "45678907",
                FirstName = "Breno",
                LastName = "Gomes",
                Email = "teste@teste.com",
                Active = true
            };
            await _contactCollection.InsertOneAsync(initialContact);

            var deleteEvent = new ContactIntegrationModel
            {
                Id = contactId,
                EventType = EventTypeEnum.Delete,
                AreaCode = "31",
                PhoneNumber = "45678907",
                FirstName = "Breno",
                LastName = "Gomes",
                Email = "teste@teste.com",
                Active = false
            };

            // Act
            await _busControl.Publish(deleteEvent);

            await Task.Delay(2000);

            // Assert
            var deletedContact = await _contactCollection.Find(c => c.Id == deleteEvent.Id).FirstOrDefaultAsync();
            Assert.NotNull(deletedContact);
            Assert.False(deletedContact.Active);
        }

        [Fact]
        public async Task Should_Not_Create_Contact_When_ContactEvent_Has_Invalid_Data()
        {
            // Arrange
            var invalidContactEvent = new ContactIntegrationModel
            {
                Id = Guid.NewGuid(),
                EventType = EventTypeEnum.Create,
                AreaCode = "",
                PhoneNumber = "45678907",
                FirstName = "Breno",
                LastName = "Gomes",
                Email = "teste@teste.com", 
                Active = true
            };

            // Act
            await _busControl.Publish(invalidContactEvent);

            await Task.Delay(2000);

            // Assert
            var contact = await _contactCollection.Find(c => c.Id == invalidContactEvent.Id).FirstOrDefaultAsync();
            Assert.Null(contact);
        }

        [Fact]
        public async Task Should_Not_Update_Contact_When_ContactEvent_Has_Invalid_Id()
        {
            // Arrange
            var invalidUpdateEvent = new ContactIntegrationModel
            {
                Id = Guid.NewGuid(),
                EventType = EventTypeEnum.Update,
                AreaCode = "31",
                PhoneNumber = "45678907",
                FirstName = "Bruno",
                LastName = "Silva",
                Email = "updated@teste.com",
                Active = true
            };

            // Act
            await _busControl.Publish(invalidUpdateEvent);

            await Task.Delay(2000);

            // Assert
            var updatedContact = await _contactCollection.Find(c => c.Id == invalidUpdateEvent.Id).FirstOrDefaultAsync();
            Assert.Null(updatedContact);
        }

        [Fact]
        public async Task Should_Not_Delete_Contact_When_ContactEvent_Has_Invalid_Id()
        {
            // Arrange
            var invalidDeleteEvent = new ContactIntegrationModel
            {
                Id = Guid.NewGuid(),
                EventType = EventTypeEnum.Delete,
                AreaCode = "31",
                PhoneNumber = "45678907",
                FirstName = "Breno",
                LastName = "Gomes",
                Email = "teste@teste.com",
                Active = false
            };

            // Act
            await _busControl.Publish(invalidDeleteEvent);

            await Task.Delay(2000);

            // Assert
            var deletedContact = await _contactCollection.Find(c => c.Id == invalidDeleteEvent.Id).FirstOrDefaultAsync();
            Assert.Null(deletedContact);
        }
    }
}
