using Postech.GroupEight.ContactIntegration.Application.Events;

namespace Postech.GroupEight.ContactIntegration.Application.Services.Interfaces
{
    public interface IContactService
    {
        Task<Guid> CreateContactHandlerAsync(ContactEvent contact);
        Task UpdateContactHandlerAsync(ContactEvent contact);
        Task DeleteContactHandlerAsync(Guid id, string areaCode);
    }
}
