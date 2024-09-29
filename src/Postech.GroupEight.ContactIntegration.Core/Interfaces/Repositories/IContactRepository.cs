using Postech.GroupEight.ContactIntegration.Core.Entities;

namespace Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories
{
    /// <summary>
    /// Represents the interface for interacting with the contact repository.
    /// </summary>
    public interface IContactRepository
    {
        /// <summary>
        /// Creates a new contact asynchronously.
        /// </summary>
        /// <param name="contact">The contact entity to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateAsync(ContactEntity contact);

        /// <summary>
        /// Updates an existing contact asynchronously.
        /// </summary>
        /// <param name="contact">The contact entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(ContactEntity contact);

        /// <summary>
        /// Deletes a contact asynchronously.
        /// </summary>
        /// <param name="id">The ID of the contact to delete.</param>
        /// <param name="areaCode">The area code of the contact to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Retrieves a contact asynchronously.
        /// </summary>
        /// <param name="id">The ID of the contact to retrieve.</param>
        /// <param name="areaCode">The area code of the contact to retrieve.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<ContactEntity> GetAsync(Guid id, string areaCode);

        /// <summary>
        /// Retrieves a contact asynchronously.
        /// </summary>
        /// <param name="id">The ID of the contact to retrieve.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<ContactEntity?> GetAsync(Guid id);
    }
}