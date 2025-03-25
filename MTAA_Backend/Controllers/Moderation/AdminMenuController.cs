using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Locations.Commands;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Domain.DTOs.Posts.Requests;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;
using System.Threading;

namespace MTAA_Backend.Api.Controllers.Moderation
{
    public class AdminMenuController : ApiController
    {
        private readonly IPostsFromGlobalPopularityRecommendationFeedService _globalPopularityService;
        private readonly IPostsFromPreferencesRecommendationFeedService _preferencesPopularityService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public AdminMenuController(IMediator mediator,
            IMapper mapper,
            IPostsFromGlobalPopularityRecommendationFeedService globalPopularityService,
            IPostsFromPreferencesRecommendationFeedService preferencesPopularityService,
            IBackgroundJobClient backgroundJobClient) : base(mediator, mapper)
        {
            _globalPopularityService = globalPopularityService;
            _preferencesPopularityService = preferencesPopularityService;
            _backgroundJobClient = backgroundJobClient;
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
    }
}
