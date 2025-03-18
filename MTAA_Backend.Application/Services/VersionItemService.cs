using Azure.Storage.Blobs.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.Entities.Versions;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Versioning;
using MTAA_Backend.Infrastructure;
using System.CodeDom;
using System.Net;

namespace MTAA_Backend.Application.Services
{
    public class VersionItemService : IVersionItemService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IStringLocalizer _localizer;

        public VersionItemService(MTAA_BackendDbContext dbContext,
            ILogger<VersionItemService> logger,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _dbContext = dbContext;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task InitializationForUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.Where(e => e.Id == userId).Include(e => e.VersionItems).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                _logger.LogError($"User not found, {userId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.NotFound);
            }

            foreach(var type in Enum.GetValues(typeof(VersionItemType)))
            {
                if (user.VersionItems.Any(e => e.Type == (VersionItemType)type)) continue;
                var newVersionItem = new VersionItem()
                {
                    Type = (VersionItemType)type,
                    UserId = user.Id,
                    Version = 1
                };
                _dbContext.VersionItems.Add(newVersionItem);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
