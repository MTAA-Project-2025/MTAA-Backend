using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Comments.Requests
{
    public class AddCommentRequest
    {
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
