using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Comments.Responses
{
    public class FullCommentResponse
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string OwnerId { get; set; }
        public string OwnerUsername { get; set; }
        public string? OwnerCustomAvatar { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ParentCommentId { get; set; }
        public int ChildCommentsCount { get; set; }
    }
}
