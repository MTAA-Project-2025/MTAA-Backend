using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Commands;
using MTAA_Backend.Application.CQRS.Groups.Chats.Commands;
using MTAA_Backend.Domain.DTOs.Groups.Chats.Requests;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Groups
{
    public class BaseGroupController : ApiController
    {
        private readonly IUserService _userService;
        public BaseGroupController(IMediator mediator,
            IMapper mapper,
            IUserService userService) : base(mediator, mapper)
        {
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("join")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> JoinGroup([FromBody] GenericIdRequest<Guid> request)
        {
            var userId = _userService.GetCurrentUserId();
            await _mediator.Send(new JoinGroup()
            {
                GroupId = request.Id,
                UserId = userId
            });
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("leave")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> LeaveGroup([FromBody] GenericIdRequest<Guid> request)
        {
            var userId = _userService.GetCurrentUserId();
            await _mediator.Send(new LeaveGroup()
            {
                GroupId = request.Id,
                UserId = userId
            });
            return Ok();
        }
    }
}
