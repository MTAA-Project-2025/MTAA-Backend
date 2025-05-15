using MTAA_Backend.Domain.DTOs.Notifications.Responses;
using MTAA_Backend.Domain.DTOs.Versioning.Responses;

namespace MTAA_Backend.Application.Services.Notifications
{
    public interface ISSEClientStorage
    {
        public Task RegisterAsync(string userId, HttpResponse response, CancellationToken cancellationToken);
        public Task SendNotificationAsync(string userId, NotificationResponse notification);
        public Task ChangeVersionAsync(string userId, VersionItemResponse versionItem);
    }
}
