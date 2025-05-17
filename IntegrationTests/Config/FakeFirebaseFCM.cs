using MTAA_Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Config
{
    public class FakeFirebaseFCM : IFCMService
    {
        public void Initialize(string credentialJsonPath)
        {
        }

        public async Task SendMessageAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null)
        {
        }

        public async Task SendMulticastAsync(List<string> deviceTokens, string title, string body, Dictionary<string, string> data = null)
        {
        }
    }
}
