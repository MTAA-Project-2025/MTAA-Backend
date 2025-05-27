using MTAA_Backend.Domain.DTOs.Notifications.Responses;
using MTAA_Backend.Domain.DTOs.Versioning.Responses;

namespace MTAA_Backend.Application.Services.Notifications
{
    public interface ISSEClientStorage
    {
        /// <summary>
        /// Registers a client for SSE notifications.
        /// </summary>
        /// <param name="userId">The ID of the user to register.</param>
        /// <param name="response">The HTTP response to use for SSE communication.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task RegisterAsync(string userId, HttpResponse response, CancellationToken cancellationToken);

        /// <summary>
        /// Sends a notification to a specific user via SSE.
        /// </summary>
        /// <param name="userId">The ID of the user to notify.</param>
        /// <param name="notification">The notification to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SendNotificationAsync(string userId, NotificationResponse notification);

        /// <summary>
        /// Updates the version information for a specific user via SSE.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="versionItem">The version item to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task ChangeVersionAsync(string userId, VersionItemResponse versionItem);
    }
}
