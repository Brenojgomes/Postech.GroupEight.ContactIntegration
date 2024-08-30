using Postech.GroupEight.ContactIntegration.Core.Enumerators;

namespace Postech.GroupEight.ContactIntegration.Application.Events
{
    public class ContactEvent
    {
        public Guid Id { get; set; }
        public string AreaCode { get; set; }
        public string Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public EventTypeEnum EventType { get; set; }

    }
}
