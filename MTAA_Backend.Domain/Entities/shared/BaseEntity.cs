using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Shared
{
    /// <summary>
    /// Provides base auditing properties for entities, tracking creation, modification, and deletion timestamps and states.
    /// Implements the <see cref="IAuditable"/> interface.
    /// </summary>
    public class BaseEntity : IAuditable
    {
        /// <summary>
        /// Gets or sets the UTC date and time when the entity was created.
        /// Defaults to the current UTC time upon instantiation.
        /// </summary>
        public DateTime DataCreationTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the UTC date and time when the entity was last marked as deleted.
        /// Nullable, as the entity might not have been deleted.
        /// </summary>
        public DateTime? DataLastDeleteTime { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the entity was last edited.
        /// Nullable, as the entity might not have been edited since creation.
        /// </summary>
        public DateTime? DataLastEditTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is considered deleted (soft delete).
        /// Defaults to false.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been edited since its creation.
        /// Defaults to false.
        /// </summary>
        public bool IsEdited { get; set; }
    }
}
