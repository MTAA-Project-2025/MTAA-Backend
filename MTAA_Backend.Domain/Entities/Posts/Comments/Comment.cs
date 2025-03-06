using MTAA_Backend.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts.Comments
{
    public class Comment : BaseEntity
    {
        public Guid Id { get; set; }

        public Post Post { get; set; }
        public Guid PostId { get; set; }
    }
}
