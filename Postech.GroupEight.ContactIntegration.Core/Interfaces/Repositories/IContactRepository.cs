using Postech.GroupEight.ContactIntegration.Core.Entities;

namespace Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories
{
    public interface IContactRepository
    {
        Task CreateAsync(Contact contact);
        Task<Contact> GetAsync(string areaCode, Guid id);
    }
}
