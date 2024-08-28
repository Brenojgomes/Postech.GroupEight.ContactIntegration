using MongoDB.Driver;
using Postech.GroupEight.ContactIntegration.Core.Entities;
using Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories;

namespace Postech.GroupEight.ContactIntegration.Infra.Persistence.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly IMongoDatabase _database;

        public ContactRepository(IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase("contacts");
        }

        private IMongoCollection<Contact> GetCollectionByAreaCode(string areaCode)
        {
            string collectionName = $"contacts_{areaCode}";
            return _database.GetCollection<Contact>(collectionName);
        }

        public async Task CreateAsync(Contact contact)
        {
            var collection = GetCollectionByAreaCode(contact.AreaCode);
            await collection.InsertOneAsync(contact);
        }

        public async Task<Contact> GetAsync(string areaCode, Guid id)
        {
            var collection = GetCollectionByAreaCode(areaCode);
            return await collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}