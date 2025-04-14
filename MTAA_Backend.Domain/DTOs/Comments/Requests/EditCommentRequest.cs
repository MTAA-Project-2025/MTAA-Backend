using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Comments.Requests
{
    public class EditCommentRequest
    {
        public Guid CommentId { get; set; }
        public string Text { get; set; }
    }
}
