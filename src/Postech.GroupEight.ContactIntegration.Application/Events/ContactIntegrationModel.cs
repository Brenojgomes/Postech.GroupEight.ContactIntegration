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
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the contact.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email of the contact.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Area code of the contact.
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// Number of the contact.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Contact creation date.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Contact modified date.
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contact event is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Type of the contact event.
        /// </summary>
        public EventTypeEnum EventType { get; set; }
    }
}
