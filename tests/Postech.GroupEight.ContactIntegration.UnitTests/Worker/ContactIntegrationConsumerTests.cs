using Bogus;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Postech.GroupEight.TechChallenge.ContactManagement.Events;
using Postech.GroupEight.ContactIntegration.Application.Services.Interfaces;
using Postech.GroupEight.ContactIntegration.Core.Enumerators;
using Postech.GroupEight.ContactIntegration.Worker.Consumers;

namespace Postech.GroupEight.ContactIntegration.UnitTests
{
    public class ContactIntegrationConsumerTests
    {
        [Fact]
        public async Task Consume_ShouldReturnCompletedTask()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ContactIntegrationConsumer>>();
            var contactServiceMock = new Mock<IContactService>();
            var consumer = new ContactIntegrationConsumer(loggerMock.Object, contactServiceMock.Object);

            var contextMock = new Mock<ConsumeContext<ContactIntegrationModel>>();
            var faker = new Faker();
            var message = new ContactIntegrationModel
            {
                Id = faker.Random.Guid(),
                AreaCode = faker.Random.String(),
                PhoneNumber = faker.Random.String(),
                FirstName = faker.Random.String(),
                LastName = faker.Random.String(),
                Email = faker.Internet.Email(),
                EventType = faker.PickRandom<EventTypeEnum>()
            };
            contextMock.Setup(x => x.Message).Returns(message);

            // Act
            var result =  consumer.Consume(contextMock.Object);

            // Assert
            Assert.True(result.IsCompletedSuccessfully);
        }
    }
}