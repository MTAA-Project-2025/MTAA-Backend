using MTAA_Backend.Domain.DTOs.Files.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Messages.Responses
{
    public class TextMessageResponse : BaseMessageResponse
    {
        public string Text { get; set; }
    }
}
