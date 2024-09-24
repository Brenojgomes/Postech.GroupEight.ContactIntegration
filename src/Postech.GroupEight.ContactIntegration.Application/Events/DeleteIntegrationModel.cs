using Postech.GroupEight.ContactIntegration.Core.Enumerators;

namespace Postech.GroupEight.TechChallenge.ContactManagement.Events
{
    public class DeleteIntegrationModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Type of the contact event.
        /// </summary>
        public EventTypeEnum EventType { get; set; }
    }
}
