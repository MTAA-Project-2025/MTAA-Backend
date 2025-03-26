using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Comments.Requests
{
    public class DislikeCommentRequest
    {
        public Guid CommentId { get; set; }
    }
}
