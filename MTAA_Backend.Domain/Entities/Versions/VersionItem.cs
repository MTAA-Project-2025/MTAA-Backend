using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Versioning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Versions
{
    /// <summary>
    /// Represents a version counter for a specific type of data associated with a user.
    /// This is used to track changes to user-specific data, enabling clients to efficiently
    /// check for updates without fetching all data.
    /// </summary>
    public class VersionItem
    {
        /// <summary>
        /// Gets or sets the type of data that this version item tracks (e.g., 'Posts', 'Followers', 'Messages').
        /// This property, along with <see cref="UserId"/>, forms the composite primary key.
        /// </summary>
        public VersionItemType Type { get; set; }

        /// <summary>
        /// Gets or sets the current version number for this data type.
        /// This integer is incremented whenever the associated data changes.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the <see cref="User"/> this version item belongs to.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the <see cref="User"/> who owns this version item.
        /// This property, along with <see cref="Type"/>, forms the composite primary key.
        /// </summary>
        public string UserId { get; set; }
    }
}
