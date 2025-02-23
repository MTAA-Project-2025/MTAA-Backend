using MTAA_Backend.Domain.DTOs.Files.Responses;
using MTAA_Backend.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Messages.Responses
{
    public class FileMessageResponse : TextMessageResponse
    {
        public MyFileResponse File { get; set; }
    }
}
