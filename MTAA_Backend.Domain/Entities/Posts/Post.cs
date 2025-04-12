using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Locations;
using MTAA_Backend.Domain.Entities.Posts.Comments;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts
{
    public class Post : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; }

        public ICollection<MyImageGroup> Images { get; set; } = new HashSet<MyImageGroup>();

        public User Owner { get; set; }
        public string OwnerId { get; set; }

        public ICollection<PostLike> Likes { get; set; } = new HashSet<PostLike>();
        public int LikesCount { get; set; }
        
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public int CommentsCount { get; set; }


        public Location? Location { get; set; }

        public double GlobalScore { get; set; }

        public ICollection<User> WatchedUsers { get; set; } = new HashSet<User>();
        public ICollection<RecommendationItem> RecommendationItems { get; set; } = new HashSet<RecommendationItem>();
    }
}
