using MongoDB.Driver;
using Moq;
using Postech.GroupEight.ContactIntegration.Core.Entities;
using Postech.GroupEight.ContactIntegration.Infra.Persistence.Repositories;
using Xunit;

namespace Postech.GroupEight.ContactIntegration.UnitTests.Infra
{
    public class ContactRepositoryTests
    {
        private readonly Mock<IMongoClient> _mongoClientMock;
        private readonly Mock<IMongoDatabase> _mongoDatabaseMock;
        private readonly Mock<IMongoCollection<ContactEntity>> _mongoCollectionMock;
        private readonly ContactRepository _contactRepository;

        public ContactRepositoryTests()
        {
            _mongoClientMock = new Mock<IMongoClient>();
            _mongoDatabaseMock = new Mock<IMongoDatabase>();
            _mongoCollectionMock = new Mock<IMongoCollection<ContactEntity>>();

            _mongoClientMock.Setup(client => client.GetDatabase(It.IsAny<string>(), null))
                .Returns(_mongoDatabaseMock.Object);

            _mongoDatabaseMock.Setup(db => db.GetCollection<ContactEntity>(It.IsAny<string>(), null))
                .Returns(_mongoCollectionMock.Object);

            _contactRepository = new ContactRepository(_mongoClientMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldInsertContact()
        {
            // Arrange
            var contact = new ContactEntity { Id = Guid.NewGuid(), AreaCode = "123" };

            // Act
            await _contactRepository.CreateAsync(contact);

            // Assert
            _mongoCollectionMock.Verify(collection => collection.InsertOneAsync(contact, null, default), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplaceContact()
        {
            // Arrange
            var contact = new ContactEntity { Id = Guid.NewGuid(), AreaCode = "123" };

            // Act
            await _contactRepository.UpdateAsync(contact);

            // Assert
            _mongoCollectionMock.Verify(collection => collection.ReplaceOneAsync(
                It.IsAny<FilterDefinition<ContactEntity>>(),
                contact,
                It.IsAny<ReplaceOptions>(),
                default), Times.Once);
        }

        //TODO: Ajustar teste delecao
        //[Fact]
        //public async Task DeleteAsync_ShouldSetContactInactive()
        //{
        //    // Arrange
        //    var contactId = Guid.NewGuid();
        //    var collectionNames = new List<string> { "contacts_123", "contacts_456" };
        //    var updateResultMock = new Mock<UpdateResult>();

        //    updateResultMock.Setup(r => r.ModifiedCount).Returns(1);

        //    _mongoDatabaseMock.Setup(db => db.ListCollectionNames(It.IsAny<ListCollectionNamesOptions>(), default))
        //        .Returns(Mock.Of<IAsyncCursor<string>>(c => c.Current == collectionNames && c.MoveNext(default) == true));

        //    _mongoCollectionMock.Setup(collection => collection.UpdateOneAsync(
        //        It.IsAny<FilterDefinition<ContactEntity>>(),
        //        It.IsAny<UpdateDefinition<ContactEntity>>(),
        //        It.IsAny<UpdateOptions>(),
        //        default)).ReturnsAsync(updateResultMock.Object);

        //    // Act
        //    await _contactRepository.DeleteAsync(contactId);

        //    // Assert
        //    _mongoCollectionMock.Verify(collection => collection.UpdateOneAsync(
        //        It.IsAny<FilterDefinition<ContactEntity>>(),
        //        It.IsAny<UpdateDefinition<ContactEntity>>(),
        //        It.IsAny<UpdateOptions>(),
        //        default), Times.Once);
        //}
    }
}
