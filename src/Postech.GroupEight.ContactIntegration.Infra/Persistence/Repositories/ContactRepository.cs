using MongoDB.Driver;
using Postech.GroupEight.ContactIntegration.Core.Entities;
using Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories;

namespace Postech.GroupEight.ContactIntegration.Infra.Persistence.Repositories
{
    /// <summary>
    /// Repository class for managing contacts.
    /// </summary>
    public class ContactRepository : IContactRepository
    {
        /// <summary>
        /// The MongoDB client.
        /// </summary>
        private readonly IMongoDatabase _database;

        public ContactRepository(IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase("contacts");
        }

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
        /// Retrieves a contact by its ID and area code.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <param name="areaCode">The area code of the contact.</param>
        /// <returns>The contact entity.</returns>
        public async Task<ContactEntity> GetAsync(Guid id, string areaCode)
        {
            var collection = GetCollectionByAreaCode(areaCode);
            return await collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <param name="contact">The contact entity to create.</param>
        public async Task CreateAsync(ContactEntity contact)
        {
            var collection = GetCollectionByAreaCode(contact.AreaCode);
            await collection.InsertOneAsync(contact);
        }

        /// <summary>
        /// Updates an existing contact.
        /// </summary>
        /// <param name="contact">The contact entity to update.</param>
        public async Task UpdateAsync(ContactEntity contact)
        {
            var collection = GetCollectionByAreaCode(contact.AreaCode);
            await collection.ReplaceOneAsync(c => c.Id == contact.Id, contact);
        }

        /// <summary>
        /// Deletes a contact by its ID and area code.
        /// </summary>
        /// <param name="Id">The ID of the contact.</param>
        /// <param name="areaCode">The area code of the contact.</param>
        public async Task DeleteAsync(Guid Id, string areaCode)
        {
            var collection = GetCollectionByAreaCode(areaCode);
            await collection.UpdateOneAsync(c => c.Id == Id, Builders<ContactEntity>.Update.Set(c => c.Active, false));
        }
    }
}