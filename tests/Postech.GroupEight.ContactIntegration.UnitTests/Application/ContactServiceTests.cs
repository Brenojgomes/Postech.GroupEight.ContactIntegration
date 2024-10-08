﻿using Microsoft.Extensions.Logging;
using Moq;
using Postech.GroupEight.TechChallenge.ContactManagement.Events;
using Postech.GroupEight.ContactIntegration.Application.Services;
using Postech.GroupEight.ContactIntegration.Core.Entities;
using Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories;

namespace Postech.GroupEight.ContactIntegration.UnitTests.Application
{
    public class ContactServiceTests
    {
        [Fact]
        public async Task CreateContactHandlerAsync_ShouldCreateNewContact_WhenContactDoesNotExist()
        {
            // Arrange
            var contactRepositoryMock = new Mock<IContactRepository>();
            var loggerMock = new Mock<ILogger<ContactService>>();

            var contactEvent = new ContactIntegrationModel
            {
                Id = Guid.NewGuid(),
                AreaCode = "123",
                PhoneNumber = "4567890",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            contactRepositoryMock
                .Setup(repo => repo.GetAsync(contactEvent.Id, contactEvent.AreaCode))
                .ReturnsAsync((ContactEntity)null);

            var contactService = new ContactService(contactRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await contactService.CreateContactHandlerAsync(contactEvent);

            // Assert
            contactRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<ContactEntity>()), Times.Once);
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("created successfully")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            Assert.Equal(contactEvent.Id, result);
        }

        [Fact]
        public async Task CreateContactHandlerAsync_ShouldLogMessage_WhenContactAlreadyExists()
        {
            // Arrange
            var contactRepositoryMock = new Mock<IContactRepository>();
            var loggerMock = new Mock<ILogger<ContactService>>();

            var contactEvent = new ContactIntegrationModel
            {
                Id = Guid.NewGuid(),
                AreaCode = "123",
                PhoneNumber = "4567890",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var existingContact = new ContactEntity
            {
                Id = contactEvent.Id,
                AreaCode = contactEvent.AreaCode,
                Number = contactEvent.PhoneNumber,
                FirstName = contactEvent.FirstName,
                LastName = contactEvent.LastName,
                Email = contactEvent.Email,
                Active = true
            };

            contactRepositoryMock
                .Setup(repo => repo.GetAsync(contactEvent.Id, contactEvent.AreaCode))
                .ReturnsAsync(existingContact);

            var contactService = new ContactService(contactRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await contactService.CreateContactHandlerAsync(contactEvent);

            // Assert
            contactRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<ContactEntity>()), Times.Never);
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("already exists")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            Assert.Equal(contactEvent.Id, result);
        }

        [Fact]
        public async Task UpdateContactHandlerAsync_ShouldUpdateContact_WhenContactExists()
        {
            // Arrange
            var contactRepositoryMock = new Mock<IContactRepository>();
            var loggerMock = new Mock<ILogger<ContactService>>();

            var contactEvent = new ContactIntegrationModel
            {
                Id = Guid.NewGuid(),
                AreaCode = "123",
                PhoneNumber = "4567890",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var existingContact = new ContactEntity
            {
                Id = contactEvent.Id,
                AreaCode = contactEvent.AreaCode,
                Number = contactEvent.PhoneNumber,
                FirstName = contactEvent.FirstName,
                LastName = contactEvent.LastName,
                Email = contactEvent.Email,
                Active = true
            };

            contactRepositoryMock
                .Setup(repo => repo.GetAsync(contactEvent.Id))
                .ReturnsAsync(existingContact);

            var contactService = new ContactService(contactRepositoryMock.Object, loggerMock.Object);

            // Act
            await contactService.UpdateContactHandlerAsync(contactEvent);

            // Assert
            contactRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ContactEntity>()), Times.Once);
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("updated successfully")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateContactHandlerAsync_ShouldThrowArgumentNullException_WhenContactIsNull()
        {
            // Arrange
            var contactRepositoryMock = new Mock<IContactRepository>();
            var loggerMock = new Mock<ILogger<ContactService>>();

            var contactService = new ContactService(contactRepositoryMock.Object, loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => contactService.UpdateContactHandlerAsync(null));
        }

        [Fact]
        public async Task UpdateContactHandlerAsync_ShouldThrowArgumentNullException_WhenContactDoesNotExist()
        {
            // Arrange
            var contactRepositoryMock = new Mock<IContactRepository>();
            var loggerMock = new Mock<ILogger<ContactService>>();

            var contactEvent = new ContactIntegrationModel
            {
                Id = Guid.NewGuid(),
                AreaCode = "123",
                PhoneNumber = "4567890",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            contactRepositoryMock
                .Setup(repo => repo.GetAsync(contactEvent.Id))
                .ReturnsAsync(() => null);

            var contactService = new ContactService(contactRepositoryMock.Object, loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => contactService.UpdateContactHandlerAsync(contactEvent));
        }

        [Fact]
        public async Task DeleteContactHandlerAsync_ShouldDeleteContact_WhenContactExists()
        {
            // Arrange
            var contactRepositoryMock = new Mock<IContactRepository>();
            var loggerMock = new Mock<ILogger<ContactService>>();

            var contactId = Guid.NewGuid();

            //contactRepositoryMock
            //    .Setup(repo => repo.GetAsync(contactId, areaCode))
            //    .ReturnsAsync(existingContact);

            var contactService = new ContactService(contactRepositoryMock.Object, loggerMock.Object);

            // Act
            await contactService.DeleteContactHandlerAsync(contactId);

            // Assert
            contactRepositoryMock.Verify(repo => repo.DeleteAsync(contactId), Times.Once);
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("deleted successfully")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}