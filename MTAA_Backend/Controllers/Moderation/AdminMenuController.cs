using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Locations.Commands;
using MTAA_Backend.Application.CQRS.Notifications.Commands;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.Commands;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Notifications.Requests;
using MTAA_Backend.Domain.DTOs.Posts.Requests;
using MTAA_Backend.Domain.DTOs.Users.Identity.Other;
using MTAA_Backend.Domain.DTOs.Users.Identity.Requests;
using MTAA_Backend.Domain.DTOs.Users.Identity.Responses;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.Locations;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Notifications;
using MTAA_Backend.Infrastructure;
using System.Net;
using System.Threading;

namespace MTAA_Backend.Api.Controllers.Moderation
{
    /// <summary>
    /// Controller for administrative and moderation tasks, including recommendation feed management, location correction, user registration bypass, post visibility, and system notifications.
    /// </summary>
    public class AdminMenuController : ApiController
    {
        private readonly IPostsFromGlobalPopularityRecommendationFeedService _globalPopularityService;
        private readonly IPostsFromPreferencesRecommendationFeedService _preferencesPopularityService;
        private readonly ILocationService _locationService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IDistributedCache _distributedCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminMenuController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs to commands.</param>
        /// <param name="globalPopularityService">The service for global post recommendation.</param>
        /// <param name="preferencesPopularityService">The service for preference-based post recommendation.</param>
        /// <param name="locationService">The service for location-related operations.</param>
        /// <param name="backgroundJobClient">The Hangfire client for scheduling background jobs.</param>
        /// <param name="distributedCache">The distributed cache for storing temporary data.</param>
        public AdminMenuController(IMediator mediator,
            IMapper mapper,
            IPostsFromGlobalPopularityRecommendationFeedService globalPopularityService,
            IPostsFromPreferencesRecommendationFeedService preferencesPopularityService,
            ILocationService locationService,
            IBackgroundJobClient backgroundJobClient,
            IDistributedCache distributedCache) : base(mediator, mapper)
        {
            _globalPopularityService = globalPopularityService;
            _preferencesPopularityService = preferencesPopularityService;
            _locationService = locationService;
            _backgroundJobClient = backgroundJobClient;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Schedules a background job to generate global post recommendations based on popularity.
        /// </summary>
        /// <returns>An empty response indicating the job was successfully scheduled.</returns>
        /// <response code="200">The recommendation job was successfully scheduled.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the Moderator role.</response>
        /// <response code="500">If an unexpected server error occurs during job scheduling.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        [Route("execute-global-posts-recommendation")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> ExecuteGlobalPostsRecommendation()
        {
            _backgroundJobClient.Enqueue(() => _globalPopularityService.RecomendPostsBackgroundJob(CancellationToken.None));
            return Ok();
        }

        /// <summary>
        /// Schedules a background job to generate post recommendations based on user preferences.
        /// </summary>
        /// <returns>An empty response indicating the job was successfully scheduled.</returns>
        /// <response code="200">The recommendation job was successfully scheduled.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the Moderator role.</response>
        /// <response code="500">If an unexpected server error occurs during job scheduling.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        [Route("execute-posts-from-preferences-recommendation")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> ExecutePostsFromPreferencesRecommendation()
        {
            _backgroundJobClient.Enqueue(() => _preferencesPopularityService.RecomendPostsBackgroundJob(CancellationToken.None));
            return Ok();
        }

        /// <summary>
        /// Schedules a background job to correct location data in the system.
        /// </summary>
        /// <returns>An empty response indicating the job was successfully scheduled.</returns>
        /// <response code="200">The location correction job was successfully scheduled.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the Moderator role.</response>
        /// <response code="500">If an unexpected server error occurs during job scheduling.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        [Route("correct-locations")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CorrectLocationsBackgroundJob()
        {
            _backgroundJobClient.Enqueue(() => _locationService.CorrectLocationsBackgroundJob());
            return Ok();
        }

        /// <summary>
        /// Bypasses email verification for user registration, allowing moderators to register users directly.
        /// </summary>
        /// <param name="request">The request containing user registration details (e.g., email, password).</param>
        /// <returns>A token DTO containing authentication details for the newly registered user.</returns>
        /// <response code="200">Returns the authentication token for the registered user.</response>
        /// <response code="400">If the request is invalid or the email is already in use.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the Moderator role.</response>
        /// <response code="500">If an unexpected server error occurs during registration.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        [Route("bypass-user-registration")]
        [ProducesResponseType(typeof(TokenDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TokenDTO>> BypassSignUpByEmail([FromBody] SignUpByEmailRequest request)
        {
            var recordId = "EmailVerificationCode_" + request.Email;
            var model = new VerificationCodeModel()
            {
                Code = "000000",
                CreationTime = DateTime.UtcNow,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                IsVerified = true
            };
            await _distributedCache.SetRecordAsync<VerificationCodeModel>(recordId, model);

            var command = _mapper.Map<SignUpByEmail>(request);
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Hides a post, making it invisible to users.
        /// </summary>
        /// <param name="id">The GUID of the post to hide.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The post was successfully hidden.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the Moderator role.</response>
        /// <response code="404">If the post does not exist.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        [Route("hide-post/{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> HidePost(Guid id)
        {
            await _mediator.Send(new HidePost()
            {
                Id = id
            });
            return Ok();
        }

        /// <summary>
        /// Unhides a previously hidden post, making it visible to users again.
        /// </summary>
        /// <param name="id">The GUID of the post to unhide.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The post was successfully unhidden.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the Moderator role.</response>
        /// <response code="404">If the post does not exist.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        [Route("unhide-post/{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UnhidePost(Guid id)
        {
            await _mediator.Send(new UnhidePost()
            {
                Id = id
            });
            return Ok();
        }

        /// <summary>
        /// Sends a system notification to specified users.
        /// </summary>
        /// <param name="request">The request containing the notification title, text, and target user IDs.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The notifications were successfully sent.</response>
        /// <response code="400">If the request is invalid or contains invalid user IDs.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the Moderator role.</response>
        /// <response code="500">If an unexpected server error occurs during notification processing.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        [HttpPost("notifications/system")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddSystemNotification([FromBody] AddSystemNotificationRequest request)
        {
            foreach (var userId in request.UserIds)
            {
                await _mediator.Send(new AddNotification
                {
                    Title = request.Title,
                    Text = request.Text,
                    Type = NotificationType.System,
                    UserId = userId,
                    PostId = null,
                    CommentId = null
                });
            }

            return Ok();
        }
    }   
}
