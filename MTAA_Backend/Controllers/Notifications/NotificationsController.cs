using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Notifications.Queries;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Application.Services.Notifications;
using MTAA_Backend.Domain.DTOs.Notifications.Responses;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Notifications;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Notifications
{
    /// <summary>
    /// Controller for managing user notifications, including retrieval and real-time subscription.
    /// </summary>
    public class NotificationsController : ApiController
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IUserService _userService;
        private readonly ISSEClientStorage _clientStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs to commands.</param>
        /// <param name="dbContext">The database context for accessing notification data.</param>
        /// <param name="localizer">The string localizer for error messages.</param>
        /// <param name="userService">The user service for user-related operations.</param>
        /// <param name="clientStorage">The storage service for managing server-sent event (SSE) clients.</param>
        public NotificationsController(IMediator mediator,
            IMapper mapper,
            MTAA_BackendDbContext dbContext,
            IStringLocalizer<ErrorMessages> localizer,
            IUserService userService,
            ISSEClientStorage clientStorage) : base(mediator, mapper)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _userService = userService;
            _clientStorage = clientStorage;
        }

        /// <summary>
        /// Retrieves paginated notifications for the authenticated user, optionally filtered by type.
        /// </summary>
        /// <param name="request">Pagination parameters (e.g., page number, page size).</param>
        /// <param name="type">The optional notification type to filter by (e.g., System, Post, Comment).</param>
        /// <returns>A collection of notifications.</returns>
        /// <response code="200">Returns the list of notifications.</response>
        /// <response code="400">If the pagination parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-notifications/{type?}")]
        [ProducesResponseType(typeof(ICollection<NotificationResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<NotificationResponse>>> GetRecommendedPosts([FromBody] PageParameters request, [FromRoute] NotificationType? type)
        {
            var res = await _mediator.Send(new GetNotifications()
            {
                PageParameters = request,
                Type = type
            });
            return Ok(res);
        }

        /// <summary>
        /// Subscribes the authenticated user to real-time notifications using server-sent events (SSE).
        /// </summary>
        /// <returns>An empty response indicating successful subscription.</returns>
        /// <response code="200">The user was successfully subscribed to notifications.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="500">If an unexpected server error occurs during subscription.</response>
        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        [Route("subscribe")]
        public async Task<IActionResult> Subscribe()
        {
            var userId = _userService.GetCurrentUserId();
            await _clientStorage.RegisterAsync(userId, Response, HttpContext.RequestAborted);
            return Ok();
        }
    }
}
