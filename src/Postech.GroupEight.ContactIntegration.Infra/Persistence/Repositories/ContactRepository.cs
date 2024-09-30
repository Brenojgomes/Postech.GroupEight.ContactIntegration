using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;
using Postech.GroupEight.ContactIntegration.Core.Entities;
using Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories;

namespace Postech.GroupEight.ContactIntegration.Infra.Persistence.Repositories
{
    /// <summary>
    /// Repository class for managing contacts.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ContactRepository(IMongoClient mongoClient) : IContactRepository
    {
        /// <summary>
        /// The MongoDB client.
        /// </summary>
        private readonly IMongoDatabase _database = mongoClient.GetDatabase("contacts");

        /// <summary>
        /// Retrieves a collection of contacts based on the provided area code.
        /// </summary>
        /// <param name="areaCode">The area code of the contacts.</param>
        /// <returns>The collection of contacts.</returns>
        private IMongoCollection<ContactEntity> GetCollectionByAreaCode(string areaCode)
        {
            string collectionName = $"contacts_{areaCode}";
            return _database.GetCollection<ContactEntity>(collectionName);
        }

        /// <summary>
        /// Retrieves all collections of contacts.
        /// </summary>
        /// <returns>The list of contact collections.</returns>
        private IEnumerable<IMongoCollection<ContactEntity>> GetAllCollections()
        {
            List<string> collectionNames = _database.ListCollectionNames().ToList();
            foreach (var collectionName in collectionNames)
            {
                yield return _database.GetCollection<ContactEntity>(collectionName);
            }
        }

        /// <summary>
        /// Retrieves a contact by its ID and area code.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <param name="areaCode">The area code of the contact.</param>
        /// <returns>The contact entity.</returns>
        public async Task<ContactEntity> GetAsync(Guid id, string areaCode)
        {
            IMongoCollection<ContactEntity> collection = GetCollectionByAreaCode(areaCode);
            return await collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves a contact by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <returns>The contact entity.</returns>
        public async Task<ContactEntity?> GetAsync(Guid id)
        {
            IEnumerable<IMongoCollection<ContactEntity>> collections = GetAllCollections();
            foreach (IMongoCollection<ContactEntity> collection in collections)
            {
                ContactEntity contactEntity = await collection.Find(c => c.Id == id).FirstOrDefaultAsync();
                if (contactEntity is not null)
                {
                    return contactEntity;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <param name="contact">The contact entity to create.</param>
        public async Task CreateAsync(ContactEntity contact)
        {
            IMongoCollection<ContactEntity> collection = GetCollectionByAreaCode(contact.AreaCode);
            await collection.InsertOneAsync(contact);
        }

        /// <summary>
        /// Updates an existing contact.
        /// </summary>
        /// <param name="contact">The contact entity to update.</param>
        public async Task UpdateAsync(ContactEntity contact)
        {
            IMongoCollection<ContactEntity> collection = GetCollectionByAreaCode(contact.AreaCode);
            await collection.ReplaceOneAsync(c => c.Id == contact.Id, contact);
        }

        /// <summary>
        /// Deletes a contact by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        public async Task DeleteAsync(Guid id)
        {
            IEnumerable<IMongoCollection<ContactEntity>> collections = GetAllCollections();
            foreach (IMongoCollection<ContactEntity> collection in collections)
            {
                DeleteResult deleteResult = await collection.DeleteOneAsync(contact => contact.Id == id);
                if (deleteResult.DeletedCount > 0)
                {
                    break;
                }
            }
        }
    }
}