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
    public class NotificationsController : ApiController
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IUserService _userService;
        private readonly ISSEClientStorage _clientStorage;
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

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-notifications/{type}")]
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
