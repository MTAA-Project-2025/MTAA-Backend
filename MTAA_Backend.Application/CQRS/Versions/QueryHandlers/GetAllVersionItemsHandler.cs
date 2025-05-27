using MediatR;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Versions.Queries;
using MTAA_Backend.Domain.Entities.Versions;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using System.Net;
using System;
using MTAA_Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.DTOs.Versioning.Responses;
using AutoMapper;

namespace MTAA_Backend.Application.CQRS.Versions.QueryHandlers
{
    /// <summary>
    /// Handles the <see cref="GetAllVersionItems"/> query to retrieve all version items associated with the current authenticated user.
    /// These version items represent the current state/version of different data categories for the user.
    /// </summary>
    public class GetAllVersionItemsHandler(ILogger<GetAllVersionItemsHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper) : IRequestHandler<GetAllVersionItems, ICollection<VersionItemResponse>>
    {
        /// <summary>
        /// Handles the <see cref="GetAllVersionItems"/> query.
        /// </summary>
        /// <param name="request">The <see cref="GetAllVersionItems"/> query request (no specific parameters needed as it uses the current user's ID).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of <see cref="VersionItemResponse"/> objects, each representing a versioned data item for the user.</returns>
        /// <exception cref="HttpException">Thrown if the user is not authenticated (user ID is null).</exception>
        public async Task<ICollection<VersionItemResponse>> Handle(GetAllVersionItems request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authenticated");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.Unauthorized);
            }

            var versionItems = await _dbContext.VersionItems.Where(v => v.UserId == userId).ToListAsync(cancellationToken);

            return _mapper.Map<ICollection<VersionItemResponse>>(versionItems);
        }
    }
}
