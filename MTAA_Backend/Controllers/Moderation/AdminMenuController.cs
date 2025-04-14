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
    public class AdminMenuController : ApiController
    {
        private readonly IPostsFromGlobalPopularityRecommendationFeedService _globalPopularityService;
        private readonly IPostsFromPreferencesRecommendationFeedService _preferencesPopularityService;
        private readonly ILocationService _locationService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IDistributedCache _distributedCache;
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

        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        [Route("execute-global-posts-recommendation")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> ExecuteGlobalPostsRecommendation()
        {
            _backgroundJobClient.Enqueue(() => _globalPopularityService.RecomendPostsBackgroundJob(CancellationToken.None));
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        [Route("execute-posts-from-preferences-recommendation")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> ExecutePostsFromPreferencesRecommendation()
        {
            _backgroundJobClient.Enqueue(() => _preferencesPopularityService.RecomendPostsBackgroundJob(CancellationToken.None));
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Moderator)]
        [Route("correct-locations")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CorrectLocationsBackgroundJob()
        {
            _backgroundJobClient.Enqueue(() => _locationService.CorrectLocationsBackgroundJob());
            return Ok();
        }

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
