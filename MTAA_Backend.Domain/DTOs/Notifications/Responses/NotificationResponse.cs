using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Resources.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Notifications.Responses
{
    public class NotificationResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public MyImageResponse Image { get; set; }
        public DateTime DataCreationTime { get; set; }
        public Guid? PostId { get; set; }
        public Guid? CommentId { get; set; }
        public string UserId { get; set; }
        public NotificationType Type { get; set; }
    }
}
