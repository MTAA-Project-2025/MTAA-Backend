using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Api.Guards.Groups;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Commands;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Groups
{
    public class ChannelController : ApiController
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IUserService _userService;

        public ChannelController(IMediator mediator,
            IMapper mapper,
            MTAA_BackendDbContext dbContext,
            IStringLocalizer<ErrorMessages> localizer,
            IUserService userService) : base(mediator, mapper)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> AddChannel([FromForm] AddChannelRequest request)
        {
            var command = _mapper.Map<AddChannel>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        #region update
        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("update-channel-description")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateChannelDescription([FromBody] UpdateChannelDescriptionRequest request)
        {
            await Guard.Against.NotChannelOwner(request.Id, _dbContext, _localizer, _userService);

            var command = _mapper.Map<UpdateChannelDescription>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("update-channel-display-name")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateChannelDisplayName([FromBody] UpdateChannelDisplayNameRequest request)
        {
            await Guard.Against.NotChannelOwner(request.Id, _dbContext, _localizer, _userService);

            var command = _mapper.Map<UpdateChannelDisplayName>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("update-channel-identification-name")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateChannelIdentificationName([FromBody] UpdateChannelIdentificationNameRequest request)
        {
            await Guard.Against.NotChannelOwner(request.Id, _dbContext, _localizer, _userService);

            var command = _mapper.Map<UpdateChannelIdentificationName>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("update-channel-image")]
        [ProducesResponseType(typeof(MyImageGroupResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateChannelImage([FromBody] UpdateChannelImageRequest request)
        {
            await Guard.Against.NotChannelOwner(request.Id, _dbContext, _localizer, _userService);

            var command = _mapper.Map<UpdateChannelImage>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("update-channel-visibility")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateChannelVisibility([FromBody] UpdateChannelVisibilityRequest request)
        {
            await Guard.Against.NotChannelOwner(request.Id, _dbContext, _localizer, _userService);

            var command = _mapper.Map<UpdateChannelVisibility>(request);
            await _mediator.Send(command);
            return Ok();
        }
        #endregion

        #region Delete
        [HttpDelete]
        [Authorize(Roles = UserRoles.User)]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateChannelDescription([FromRoute] Guid id)
        {
            await Guard.Against.NotChannelOwner(id, _dbContext, _localizer, _userService);

            await _mediator.Send(new DeleteGroup
            {
                Id = id
            });
            return Ok();
        }
        #endregion
    }
}
