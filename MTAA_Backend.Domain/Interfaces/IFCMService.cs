using MTAA_Backend.Domain.Entities.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces
{
    public interface IFCMService
    {
        public void Initialize(string credentialJsonPath);
        public Task SendMessageAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null);
        public Task SendMulticastAsync(List<string> deviceTokens, string title, string body, Dictionary<string, string> data = null);
    }
}
