using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts.Comments
{
    public class CommentInteraction : BaseEntity
    {
        public User User { get; set; }
        public string UserId { get; set; }

        public Comment Comment { get; set; }
        public Guid CommentId { get; set; }

        public CommentInteractionType Type { get; set; }
    }
}
