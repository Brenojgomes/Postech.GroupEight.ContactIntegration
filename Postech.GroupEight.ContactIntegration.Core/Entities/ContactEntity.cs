namespace Postech.GroupEight.ContactIntegration.Core.Entities
{
    /// <summary>
    /// Represents a contact entity.
    /// </summary>
    public class ContactEntity : EntityBase
    {
        /// <summary>
        /// The area code of the contact.
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// The number of the contact.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// The first name of the contact.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the contact.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email of the contact.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A value indicating whether the contact is active.
        /// </summary>
        public bool Active { get; set; }
    }
}
