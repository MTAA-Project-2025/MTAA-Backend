using MTAA_Backend.Domain.Resources.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Entities.Messages
{
    public class TextMessage : BaseMessage
    {
        public string ShortText { get; set; }
        public string Text { get; set; }

        public TextMessage(string type = MessageTypes.TextMesage) : base(type) { }
    }
}
