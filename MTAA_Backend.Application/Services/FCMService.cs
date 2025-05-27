using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MTAA_Backend.Domain.Interfaces;

namespace MTAA_Backend.Application.Services
{
    //GPT
    /// <summary>
    /// Provides services for sending push notifications using Firebase Cloud Messaging (FCM).
    /// </summary>
    public class FCMService : IFCMService
    {
        /// <summary>
        /// Initializes the Firebase app with the specified credential file.
        /// </summary>
        /// <param name="credentialJsonPath">The path to the Firebase credential JSON file.</param>
        public void Initialize(string credentialJsonPath)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(credentialJsonPath)
            });
        }

        /// <summary>
        /// Sends a push notification to a single device using FCM.
        /// </summary>
        /// <param name="deviceToken">The device token to send the notification to.</param>
        /// <param name="title">The title of the notification.</param>
        /// <param name="body">The body of the notification.</param>
        /// <param name="data">Optional key-value pairs to include in the notification.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Sends a push notification to multiple devices using FCM.
        /// </summary>
        /// <param name="deviceTokens">The list of device tokens to send the notification to.</param>
        /// <param name="title">The title of the notification.</param>
        /// <param name="body">The body of the notification.</param>
        /// <param name="data">Optional key-value pairs to include in the notification.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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
