using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Shared;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Images
{
    public class MyImageGroup : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }

        public ICollection<MyImage> Images { get; set; } = new HashSet<MyImage>();

        public UserAvatar? UserAvatar { get; set; }
        public string? UserAvatarId { get; set; }

        public Channel? Channel { get; set; }
        public Guid? ChannelId { get; set; }

        public ImagesMessage? Message { get; set; }
        public Guid? MessageId { get; set; }

        public Post? Post { get; set; }
        public Guid? PostId { get; set; }
        public int Position { get; set; } = 0;
    }
}
