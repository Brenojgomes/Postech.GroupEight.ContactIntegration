using Postech.GroupEight.ContactIntegration.Core.Entities;

namespace Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories
{
    public interface IContactRepository
    {
        Task CreateAsync(Contact contact);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(Guid id, string areaCode);
        Task<Contact> GetAsync(Guid id, string areaCode);
    }
}
