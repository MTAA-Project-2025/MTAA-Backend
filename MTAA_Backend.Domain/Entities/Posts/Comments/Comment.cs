using MTAA_Backend.Domain.Entities.Notifications;
using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Posts.Comments
{
    public class Comment : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Text { get; set; } = string.Empty;
        public int LikesCount { get; set; } = 0;
        public int DislikesCount { get; set; } = 0;

        public Post Post { get; set; }
        public Guid PostId { get; set; }

        public User Owner { get; set; } = null!;
        public string OwnerId { get; set; }

        public Comment? ParentComment { get; set; }
        public Guid? ParentCommentId { get; set; }

        public ICollection<Comment> ChildComments { get; set; } = new HashSet<Comment>();
        public int ChildCommentsCount { get; set; } = 0;

        public ICollection<CommentInteraction> CommentInteractions { get; set; } = new HashSet<CommentInteraction>();

        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
    }
}
