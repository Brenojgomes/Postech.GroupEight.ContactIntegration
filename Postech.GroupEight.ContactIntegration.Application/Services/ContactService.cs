using Microsoft.Extensions.Logging;
using Postech.GroupEight.ContactIntegration.Application.Events;
using Postech.GroupEight.ContactIntegration.Application.Services.Interfaces;
using Postech.GroupEight.ContactIntegration.Core.Entities;
using Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories;

namespace Postech.GroupEight.ContactIntegration.Application.Services
{
    /// <summary>
    /// Service class for managing contacts.
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILogger<ContactService> _logger;

        public ContactService(IContactRepository contactRepository, ILogger<ContactService> logger)
        {
            _contactRepository = contactRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <param name="contact">The contact event.</param>
        /// <returns>The ID of the created contact.</returns>
        public async Task<Guid> CreateContactHandlerAsync(ContactEvent contact)
        {
            if (contact is null) throw new ArgumentNullException(nameof(contact));

            var contactEntity = await _contactRepository.GetAsync(contact.Id, contact.AreaCode);

            if (contactEntity is null)
            {
                contactEntity = new ContactEntity
                {
                    Id = contact.Id,
                    CreatedAt = DateTime.UtcNow,
                    AreaCode = contact.AreaCode,
                    Number = contact.Number,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Active = contact.Active,
                };

                await _contactRepository.CreateAsync(contactEntity);
                _logger.Log(LogLevel.Information, $"Contact Id: {contactEntity.Id} created successfully");
            }
            else
            {
                _logger.Log(LogLevel.Information, $"Contact Id: {contactEntity.Id} already exists");
            }

            return contactEntity.Id;
        }

        /// <summary>
        /// Updates an existing contact.
        /// </summary>
        /// <param name="contact">The contact event.</param>
        public async Task UpdateContactHandlerAsync(ContactEvent contact)
        {
            if (contact is null) throw new ArgumentNullException(nameof(contact));

            var contactEntity = await _contactRepository.GetAsync(contact.Id, contact.AreaCode);

            if (contactEntity is null)
                throw new ArgumentNullException(nameof(contactEntity));

            contactEntity.AreaCode = contact.AreaCode;
            contactEntity.Number = contact.Number;
            contactEntity.FirstName = contact.FirstName;
            contactEntity.LastName = contact.LastName;
            contactEntity.Email = contact.Email;
            contactEntity.Active = contact.Active;
            contactEntity.ModifiedAt = DateTime.UtcNow;

            await _contactRepository.UpdateAsync(contactEntity);
            _logger.Log(LogLevel.Information, $"Contact Id: {contactEntity.Id} updated successfully");
        }

        /// <summary>
        /// Deletes a contact.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <param name="areaCode">The area code of the contact.</param>
        public async Task DeleteContactHandlerAsync(Guid id, string areaCode)
        {
            var contactEntity = await _contactRepository.GetAsync(id, areaCode);

            if (contactEntity is null)
                throw new ArgumentNullException(nameof(contactEntity));

            await _contactRepository.DeleteAsync(id, areaCode);
            _logger.Log(LogLevel.Information, $"Contact Id: {contactEntity.Id} deleted successfully");
        }
    }
}
