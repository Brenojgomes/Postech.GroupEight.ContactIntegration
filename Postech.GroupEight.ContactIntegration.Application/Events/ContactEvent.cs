using Postech.GroupEight.ContactIntegration.Core.Enumerators;

namespace Postech.GroupEight.ContactIntegration.Application.Events
{
    /// <summary>
    /// Represents a contact.
    /// </summary>
    public class ContactEvent
    {
        /// <summary>
        /// ID of the contact.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Area code of the contact.
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// Number of the contact.
        /// </summary>
        public string Number { get; set; }

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
        /// Gets or sets a value indicating whether the contact event is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Type of the contact event.
        /// </summary>
        public EventTypeEnum EventType { get; set; }
    }
}
