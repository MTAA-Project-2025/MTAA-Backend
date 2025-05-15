using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MTAA_Backend.Domain.Interfaces;

namespace MTAA_Backend.Application.Services
{
    //GPT
    public class FCMService : IFCMService
    {
        public void Initialize(string credentialJsonPath)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(credentialJsonPath)
            });
        }

        public async Task SendMessageAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null)
        {
            var message = new Message()
            {
                Token = deviceToken,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Data = data
            };

            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Successfully sent message: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send message: {ex.Message}");
            }
        }

        public async Task SendMulticastAsync(List<string> deviceTokens, string title, string body, Dictionary<string, string> data = null)
        {
            var message = new MulticastMessage()
            {
                Tokens = deviceTokens,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Data = data
            };

            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
            Console.WriteLine($"{response.SuccessCount} messages were sent successfully");
        }
    }
}
