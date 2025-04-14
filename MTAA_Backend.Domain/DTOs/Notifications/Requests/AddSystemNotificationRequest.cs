using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Notifications.Requests
{
    public class AddSystemNotificationRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public List<string> UserIds { get; set; }
    }
}
