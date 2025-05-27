using Microsoft.AspNetCore.Identity;
using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Entities.Notifications;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Posts.Comments;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Versions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Users
{
    /// <summary>
    /// Represents a user in the application, extending the default ASP.NET Core IdentityUser.
    /// It includes additional properties for user profiles, relationships, content creation,
    /// and participation in various features like groups, messages, and recommendations.
    /// Implements <see cref="IAuditable"/> for tracking creation, modification, and deletion.
    /// </summary>
    public class User : IdentityUser, IAuditable
    {
        /// <summary>
        /// Gets or sets the display name of the user, which can be different from their username.
        /// Defaults to an empty string.
        /// </summary>
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// Gets or sets the birth date of the user. Nullable.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the user was last seen active in the application.
        /// Defaults to the current UTC time upon instantiation.
        /// </summary>
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;

        // IAuditable Properties
        /// <summary>
        /// Gets or sets the UTC date and time when the user entity was created.
        /// Defaults to the current UTC time upon instantiation.
        /// </summary>
        public DateTime DataCreationTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the UTC date and time when the user entity was last marked as deleted (soft delete). Nullable.
        /// </summary>
        public DateTime? DataLastDeleteTime { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the user entity was last edited. Nullable.
        /// </summary>
        public DateTime? DataLastEditTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user account is considered deleted (soft delete).
        /// Defaults to false.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's data has been edited since creation.
        /// Defaults to false.
        /// </summary>
        public bool IsEdited { get; set; }

        // Navigation Properties

        /// <summary>
        /// Gets or sets the collection of <see cref="BaseGroup"/>s the user is a member of.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<BaseGroup> Groups { get; set; } = new HashSet<BaseGroup>();

        /// <summary>
        /// Gets or sets the collection of <see cref="BaseMessage"/>s sent by this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<BaseMessage> Messages { get; set; } = new HashSet<BaseMessage>();

        /// <summary>
        /// Gets or sets the collection of <see cref="UserGroupMembership"/> entries linking this user to groups.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<UserGroupMembership> UserGroupMemberships { get; set; } = new HashSet<UserGroupMembership>();

        /// <summary>
        /// Gets or sets the <see cref="UserAvatar"/> associated with this user. Nullable.
        /// </summary>
        public UserAvatar? Avatar { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the associated <see cref="UserAvatar"/>. Nullable.
        /// </summary>
        public Guid? AvatarId { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Channel"/>s owned by this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<Channel> OwnedChannels { get; set; } = new HashSet<Channel>();

        /// <summary>
        /// Gets or sets the collection of <see cref="UserRelationship"/>s where this user is User1.
        /// Represents relationships initiated by this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<UserRelationship> UserRelationships1 { get; set; } = new HashSet<UserRelationship>();

        /// <summary>
        /// Gets or sets the collection of <see cref="UserRelationship"/>s where this user is User2.
        /// Represents relationships where this user is the target.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<UserRelationship> UserRelationships2 { get; set; } = new HashSet<UserRelationship>();

        /// <summary>
        /// Gets or sets the collection of <see cref="Post"/>s created by this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<Post> CreatedPosts { get; set; } = new HashSet<Post>();

        /// <summary>
        /// Gets or sets the collection of <see cref="PostLike"/>s made by this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<PostLike> LikedPosts { get; set; } = new HashSet<PostLike>();

        /// <summary>
        /// Gets or sets the collection of <see cref="Post"/>s that this user has watched or saved.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<Post> WatchedPosts { get; set; } = new HashSet<Post>();

        /// <summary>
        /// Gets or sets the collection of <see cref="Comment"/>s created by this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<Comment> CreatedComments { get; set; } = new HashSet<Comment>();

        /// <summary>
        /// Gets or sets the collection of <see cref="CommentInteraction"/>s made by this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<CommentInteraction> CommentInteractions { get; set; } = new HashSet<CommentInteraction>();

        /// <summary>
        /// Gets or sets the index for the user's global recommendations feed.
        /// This could be used to track pagination or progress through a feed.
        /// </summary>
        public int GlobalRecommendationsFeedIndex { get; set; } = 0;

        /// <summary>
        /// Gets or sets the collection of <see cref="LocalRecommendationFeed"/>s specific to this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<LocalRecommendationFeed> LocalRecommendationFeeds { get; set; } = new HashSet<LocalRecommendationFeed>();

        /// <summary>
        /// Gets or sets the collection of <see cref="SharedRecommendationFeed"/>s related to this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<SharedRecommendationFeed> SharedRecommendationFeeds { get; set; } = new HashSet<SharedRecommendationFeed>();

        /// <summary>
        /// Gets or sets the collection of <see cref="VersionItem"/>s tracking different data versions for this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<VersionItem> VersionItems { get; set; } = new HashSet<VersionItem>();

        /// <summary>
        /// Gets or sets the collection of <see cref="Notification"/>s belonging to this user.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();

        /// <summary>
        /// Gets or sets the collection of <see cref="FirebaseItem"/>s associated with this user,
        /// typically for Firebase Cloud Messaging tokens or other Firebase-related data.
        /// Initialized as a new HashSet.
        /// </summary>
        public ICollection<FirebaseItem> FirebaseItems { get; set; } = new HashSet<FirebaseItem>();
    }
}
