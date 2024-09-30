using Postech.GroupEight.ContactIntegration.Core.Enumerators;

namespace Postech.GroupEight.TechChallenge.ContactManagement.Events
{
    /// <summary>
    /// Represents a contact.
    /// </summary>
    public class ContactIntegrationModel
    {
        /// <summary>
        /// ID of the contact.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First name of the contact.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the contact.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email of the contact.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Area code of the contact.
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// Number of the contact.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Contact creation date.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Contact modified date.
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// Type of the contact event.
        /// </summary>
        public EventTypeEnum EventType { get; set; }
    }
}