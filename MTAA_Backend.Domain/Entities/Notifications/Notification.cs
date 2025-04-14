using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Posts.Comments;
using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Notifications
{
    public class Notification : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }
        public string Text { get; set; }

        public NotificationType Type { get; set; }

        public Guid? PostId { get; set; }
        public Post Post { get; set; }

        public Guid? CommentId { get; set; }
        public Comment Comment { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
