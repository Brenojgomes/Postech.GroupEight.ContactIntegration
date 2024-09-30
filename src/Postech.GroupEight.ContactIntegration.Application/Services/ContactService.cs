using Microsoft.Extensions.Logging;
using Postech.GroupEight.TechChallenge.ContactManagement.Events;
using Postech.GroupEight.ContactIntegration.Application.Services.Interfaces;
using Postech.GroupEight.ContactIntegration.Core.Entities;
using Postech.GroupEight.ContactIntegration.Core.Interfaces.Repositories;

namespace Postech.GroupEight.ContactIntegration.Application.Services
{
    /// <summary>
    /// Service class for managing contacts.
    /// </summary>
    public class ContactService(IContactRepository contactRepository, ILogger<ContactService> logger) : IContactService
    {
        private readonly IContactRepository _contactRepository = contactRepository;
        private readonly ILogger<ContactService> _logger = logger;

        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <param name="contact">The contact event.</param>
        /// <returns>The ID of the created contact.</returns>
        public async Task<Guid> CreateContactHandlerAsync(ContactIntegrationModel contact)
        {
            ArgumentNullException.ThrowIfNull(contact);
            ContactEntity contactEntity = await _contactRepository.GetAsync(contact.Id, contact.AreaCode);
            if (contactEntity is null)
            {
                contactEntity = new ContactEntity
                {
                    Id = contact.Id,
                    CreatedAt = contact.CreatedAt,
                    AreaCode = contact.AreaCode,
                    Number = contact.PhoneNumber,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    Active = true,
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
        public async Task UpdateContactHandlerAsync(ContactIntegrationModel contact)
        {
            ArgumentNullException.ThrowIfNull(contact);
            ContactEntity? contactEntity = await _contactRepository.GetAsync(contact.Id);
            ArgumentNullException.ThrowIfNull(contactEntity);          
            contactEntity.Number = contact.PhoneNumber;
            contactEntity.FirstName = contact.FirstName;
            contactEntity.LastName = contact.LastName;
            contactEntity.Email = contact.Email;
            contactEntity.ModifiedAt = contact.ModifiedAt;
            if (contactEntity.AreaCode.Equals(contact.AreaCode))
            {
                await _contactRepository.UpdateAsync(contactEntity);
            }
            else 
            {
                contactEntity.AreaCode = contact.AreaCode;
                await _contactRepository.DeleteAsync(contactEntity.Id);
                await _contactRepository.CreateAsync(contactEntity);
            }     
            _logger.Log(LogLevel.Information, $"Contact Id: {contactEntity.Id} updated successfully");
        }

        /// <summary>
        /// Deletes a contact.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <param name="areaCode">The area code of the contact.</param>
        public async Task DeleteContactHandlerAsync(Guid id)
        {
            await _contactRepository.DeleteAsync(id);
            _logger.Log(LogLevel.Information, $"Contact Id: {id} deleted successfully");
        }
    }
}