using Microsoft.AspNetCore.Identity;
using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Entities.Posts;
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
    public class User : IdentityUser, IAuditable
    {
        public string DisplayName { get; set; } = "";
        public DateTime? BirthDate { get; set; }

        public DateTime LastSeen { get; set; } = DateTime.UtcNow;

        public DateTime DataCreationTime { get; set; } = DateTime.UtcNow;
        public DateTime? DataLastDeleteTime { get; set; }
        public DateTime? DataLastEditTime { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsEdited { get; set; }

        public ICollection<BaseGroup> Groups { get; set; } = new HashSet<BaseGroup>();
        public ICollection<BaseMessage> Messages { get; set; } = new HashSet<BaseMessage>();
        public ICollection<UserGroupMembership> UserGroupMemberships { get; set; } = new HashSet<UserGroupMembership>();

        public UserAvatar? Avatar { get; set; }
        public Guid? AvatarId { get; set; }

        public ICollection<Channel> OwnedChannels { get; set; } = new HashSet<Channel>();


        public ICollection<UserRelationship> UserRelationships1 { get; set; } = new HashSet<UserRelationship>();
        public ICollection<UserRelationship> UserRelationships2 { get; set; } = new HashSet<UserRelationship>();

        public ICollection<Post> CreatedPosts { get; set; } = new HashSet<Post>();
        public ICollection<PostLike> LikedPosts { get; set; } = new HashSet<PostLike>();
        public ICollection<Post> WatchedPosts { get; set; } = new HashSet<Post>();

        public int GlobalRecommendationsFeedIndex = 0;
        public ICollection<LocalRecommendationFeed> LocalRecommendationFeeds { get; set; } = new HashSet<LocalRecommendationFeed>();
        public ICollection<SharedRecommendationFeed> SharedRecommendationFeeds { get; set; } = new HashSet<SharedRecommendationFeed>();

        public ICollection<VersionItem> VersionItems { get; set; } = new HashSet<VersionItem>();
    }
}