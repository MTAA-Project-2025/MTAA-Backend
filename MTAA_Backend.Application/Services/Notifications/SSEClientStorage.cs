﻿using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.DTOs.Notifications.Responses;
using MTAA_Backend.Domain.DTOs.Versioning.Responses;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Infrastructure;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace MTAA_Backend.Application.Services.Notifications
{
    // Generated by GPT
    public class SSEClientStorage : ISSEClientStorage
    {
        private readonly IFCMService _fcmService;
        private readonly IServiceScopeFactory _scopeFactory;
        public SSEClientStorage(IFCMService fcmService,
            IServiceScopeFactory scopeFactory)
        {
            _fcmService = fcmService;
            _scopeFactory = scopeFactory;
        }
        private readonly ConcurrentDictionary<string, List<HttpResponse>> _clients = new();

        public async Task RegisterAsync(string userId, HttpResponse response, CancellationToken cancellationToken)
        {
            response.Headers.Add("Content-Type", "text/event-stream");

            _clients.AddOrUpdate(userId,
                _ => new List<HttpResponse> { response },
                (_, list) => { list.Add(response); return list; });

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (TaskCanceledException) { }
            finally
            {
                if (_clients.TryGetValue(userId, out var list))
                {
                    list.Remove(response);
                }
            }
        }

        public async Task SendNotificationAsync(string userId, NotificationResponse notification)
        {
            if (!_clients.TryGetValue(userId, out var responses))
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    try
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<MTAA_BackendDbContext>();
                        var fItems = await dbContext.FirebaseItems.Where(e => e.UserId == userId).ToListAsync();
                        await _fcmService.SendMulticastAsync(fItems.Select(e => e.Token).ToList(), notification.Title, notification.Text);
                    }
                    catch
                    {
                        return;
                    }
                }
                return;
            }

            var json = JsonSerializer.Serialize(notification);
            var data = $"event: notification\ndata: {json}\n\n";
            var bytes = Encoding.UTF8.GetBytes(data);

            if (responses.Count == 0)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    try
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<MTAA_BackendDbContext>();
                        var fItems = await dbContext.FirebaseItems.Where(e => e.UserId == userId).ToListAsync();
                        await _fcmService.SendMulticastAsync(fItems.Select(e => e.Token).ToList(), notification.Title, notification.Text);
                    }
                    catch
                    {
                        return;
                    }
                }
                return;
            }
            foreach (var response in responses.ToList())
            {
                try
                {
                    await response.Body.WriteAsync(bytes, 0, bytes.Length);
                    await response.Body.FlushAsync();
                }
                catch
                {
                    responses.Remove(response);
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        try
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<MTAA_BackendDbContext>();
                            var fItems = await dbContext.FirebaseItems.Where(e => e.UserId == userId).ToListAsync();
                            await _fcmService.SendMulticastAsync(fItems.Select(e => e.Token).ToList(), notification.Title, notification.Text);
                        }
                        catch { }
                    }
                }
            }
        }

        public async Task ChangeVersionAsync(string userId, VersionItemResponse versionItem)
        {
            if (!_clients.TryGetValue(userId, out var responses)) return;

            var json = JsonSerializer.Serialize(versionItem);
            var data = $"event: version\ndata: {json}\n\n";
            var bytes = Encoding.UTF8.GetBytes(data);

            foreach (var response in responses.ToList())
            {
                try
                {
                    await response.Body.WriteAsync(bytes, 0, bytes.Length);
                    await response.Body.FlushAsync();
                }
                catch
                {
                    responses.Remove(response);
                }
            }
        }
    }
}
