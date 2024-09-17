namespace Postech.GroupEight.ContactIntegration.Core.Interfaces.Entities
{
    /// <summary>
    /// Represents an entity in the system.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Unique identifier of the entity.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Creation date and time of the entity.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Last modification date and time of the entity.
        /// </summary>
        DateTime? ModifiedAt { get; set; }
    }
}
