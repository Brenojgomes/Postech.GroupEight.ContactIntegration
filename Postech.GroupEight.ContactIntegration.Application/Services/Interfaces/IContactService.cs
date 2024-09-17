using Postech.GroupEight.TechChallenge.ContactManagement.Events;

namespace Postech.GroupEight.ContactIntegration.Application.Services.Interfaces
{
    /// <summary>
    /// Represents a contact service.
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <param name="contact">The contact event.</param>
        /// <returns>The ID of the created contact.</returns>
        Task<Guid> CreateContactHandlerAsync(ContactEvent contact);

        /// <summary>
        /// Updates an existing contact.
        /// </summary>
        /// <param name="contact">The contact event.</param>
        Task UpdateContactHandlerAsync(ContactEvent contact);

        /// <summary>
        /// Deletes a contact by ID and area code.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <param name="areaCode">The area code of the contact.</param>
        Task DeleteContactHandlerAsync(Guid id, string areaCode);
    }
}
