using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Api.Guards.Groups;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Groups.UserGroupMemberships.Commands;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Groups
{
    public class UserGroupMembershipController : ApiController
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IUserService _userService;
        public UserGroupMembershipController(IMediator mediator,
            IMapper mapper,
            MTAA_BackendDbContext dbContext,
            IStringLocalizer<ErrorMessages> localizer,
            IUserService userService) : base(mediator, mapper)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _userService = userService;
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("allow-notifications")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> AllowUserGroupMembershipNotifications([FromForm] GenericIdRequest<Guid> request)
        {
            await Guard.Against.NotUserGroupMembershipOwner(request.Id, _dbContext, _localizer, _userService);

            var command = _mapper.Map<AllowUserGroupMembershipNotifications>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("forbid-notifications")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ForbidUserGroupMembershipNotifications([FromForm] GenericIdRequest<Guid> request)
        {
            await Guard.Against.NotUserGroupMembershipOwner(request.Id, _dbContext, _localizer, _userService);

            var command = _mapper.Map<ForbidUserGroupMembershipNotifications>(request);
            await _mediator.Send(command);
            return Ok();
        }


        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("archive")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ArchiveUserGroupMembership([FromForm] GenericIdRequest<Guid> request)
        {
            await Guard.Against.NotUserGroupMembershipOwner(request.Id, _dbContext, _localizer, _userService);

            var command = _mapper.Map<ArchiveUserGroupMembership>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("unarchive")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UnarchiveUserGroupMembership([FromForm] GenericIdRequest<Guid> request)
        {
            await Guard.Against.NotUserGroupMembershipOwner(request.Id, _dbContext, _localizer, _userService);

            var command = _mapper.Map<UnarchiveUserGroupMembership>(request);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
