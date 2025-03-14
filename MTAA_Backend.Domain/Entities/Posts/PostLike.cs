using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts
{
    public class PostLike
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public User User { get; set; }
        public string UserId { get; set; }

        public Post Post { get; set; }
        public Guid PostId { get; set; }

        public DateTime DataCreationTime { get; set; } = DateTime.UtcNow;
    }
}
