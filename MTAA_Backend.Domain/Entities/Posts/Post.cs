using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Locations;
using MTAA_Backend.Domain.Entities.Posts.Comments;
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
        public Guid Id { get; set; }
        public string Description { get; set; }

        public ICollection<MyImageGroup> Images { get; set; } = new HashSet<MyImageGroup>();

        public User Owner { get; set; }
        public string OwnerId { get; set; }

        public ICollection<User> LikedUsers { get; set; } = new HashSet<User>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        
        public Location? Location { get; set; }
        public Guid? LocationId { get; set; }
    }
}
