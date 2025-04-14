using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Resources.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Messages
{
    public class ImagesMessage : TextMessage
    {
        public ICollection<MyImageGroup> Images { get; set; } = new HashSet<MyImageGroup>();

        public ImagesMessage(string type = MessageTypes.ImagesMessage) : base(type) { }
    }
}
