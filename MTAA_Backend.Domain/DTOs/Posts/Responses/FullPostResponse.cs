using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Posts.Comments;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;

namespace MTAA_Backend.Domain.DTOs.Posts.Responses
{
    public class FullPostResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public ICollection<MyImageGroupResponse> Images { get; set; }

        public PublicSimpleAccountResponse Owner { get; set; }

        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsLiked { get; set; }

        public Guid? LocationId { get; set; }

        public DateTime DataCreationTime { get; set; }
    }
}
