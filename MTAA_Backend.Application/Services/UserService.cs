using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Services
{
    /// <summary>
    /// Provides services for managing user-related operations, such as retrieving the current user.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;

        /// <summary>
        /// Initializes a new instance of the UserService class.
        /// </summary>
        /// <param name="httpContextAccessor">The accessor for HTTP context information.</param>
        /// <param name="userManager">The manager for user-related operations.</param>
        /// <param name="localizer">The localizer for error messages.</param>
        public UserService(IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _localizer = localizer;
        }

        /// <summary>
        /// Retrieves the ID of the current user from the HTTP context.
        /// </summary>
        /// <returns>The user ID, or null if not found.</returns>
        public string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value;
        }

        /// <summary>
        /// Retrieves the current user based on their ID from the HTTP context.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning the current user.</returns>
        /// <exception cref="HttpException">Thrown if the user is not authorized or not found.</exception>
        public async Task<User> GetCurrentUser()
        {
            var id = GetCurrentUserId();
            if (id == null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.BadRequest);
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.BadRequest);
            }
            return user;
        }
    }
}
