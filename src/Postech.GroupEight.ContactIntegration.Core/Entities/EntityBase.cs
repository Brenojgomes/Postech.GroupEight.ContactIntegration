using Postech.GroupEight.ContactIntegration.Core.Interfaces.Entities;

namespace Postech.GroupEight.ContactIntegration.Core.Entities
{
    /// <summary>
    /// Represents the base entity class.
    /// </summary>
    public class EntityBase : IEntity
    {
        /// <summary>
        /// The unique identifier of the entity.
        /// </summary>
        public Guid Id { get; set;}

        /// <summary>
        /// The creation date and time of the entity.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The last modification date and time of the entity.
        /// </summary>
        public DateTime? ModifiedAt { get; set; }
    }
}
