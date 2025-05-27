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
    /// <summary>
    /// Provides services for managing version items associated with users.
    /// </summary>
    public class VersionItemService : IVersionItemService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IStringLocalizer _localizer;

        /// <summary>
        /// Initializes a new instance of the VersionItemService class.
        /// </summary>
        /// <param name="dbContext">The database context for data operations.</param>
        /// <param name="logger">The logger for recording errors.</param>
        /// <param name="localizer">The localizer for error messages.</param>
        public VersionItemService(MTAA_BackendDbContext dbContext,
            ILogger<VersionItemService> logger,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _dbContext = dbContext;
            _logger = logger;
            _localizer = localizer;
        }

        /// <summary>
        /// Initializes version items for a user, creating entries for each version item type if they do not exist.
        /// </summary>
        /// <param name="userId">The ID of the user to initialize version items for.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="HttpException">Thrown if the user is not found.</exception>
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
