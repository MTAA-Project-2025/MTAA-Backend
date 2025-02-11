using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Groups.Chats.Requests
{
    public class AddChatRequest
    {
        public string Visibility { get; set; }
        public string IdentificationName { get; set; }
    }
}
