using MTAA_Backend.Domain.Entities.Files;
using MTAA_Backend.Domain.Resources.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Messages
{
    public class VoiceMessage : BaseMessage
    {
        public MyFile File { get; set; }
        public Guid FileId { get; set; }

        public TimeSpan Duration { get; set; }

        public VoiceMessage(string type = MessageTypes.VoiceMessage) : base(type) { }
    }
}
