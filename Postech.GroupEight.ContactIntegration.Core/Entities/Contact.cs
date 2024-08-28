namespace Postech.GroupEight.ContactIntegration.Core.Entities
{
    public class Contact : EntityBase
    {
        public string AreaCode { get; set; }
        public string Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
