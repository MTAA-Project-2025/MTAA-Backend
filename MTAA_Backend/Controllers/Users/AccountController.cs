using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Application.CQRS.Users.Relationships.Commands;
using MTAA_Backend.Application.CQRS.Users.Relationships.Queries;
using MTAA_Backend.Application.CQRS.Versions.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Entities.Versions;
using MTAA_Backend.Domain.Resources.Customers;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Users
{
    public class AccountController : ApiController
    {
        public AccountController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        #region get
        [HttpGet]
        [Route("public-full-account/{userId}")]
        [ProducesResponseType(typeof(PublicFullAccountResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PublicFullAccountResponse>> PublicGetFullAccount([FromRoute] string userId)
        {
            var query = new PublicGetFullAccount()
            {
                UserId = userId
            };
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpGet("followers")]
        [ProducesResponseType(typeof(ICollection<PublicSimpleAccountResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFollowers([FromQuery] PageParameters pageParameters)
        {
            var query = new GetFollowers { PageParameters = pageParameters };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("friends")]
        [ProducesResponseType(typeof(ICollection<PublicSimpleAccountResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFriends([FromQuery] PageParameters pageParameters)
        {
            var query = new GetFriends { PageParameters = pageParameters };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("all-versions")]
        [ProducesResponseType(typeof(IEnumerable<VersionItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllVersions(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllVersionItems(), cancellationToken);
            return Ok(result);
        }
        #endregion

        #region update
        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("custom-update-account-avatar")]
        [ProducesResponseType(typeof(MyImageGroupResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MyImageGroupResponse>> CustomUpdateAccountAvatar([FromForm] CustomUpdateAccountAvatarRequest request)
        {
            var command = _mapper.Map<CustomUpdateAccountAvatar>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("preset-update-account-avatar")]
        [ProducesResponseType(typeof(MyImageGroupResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MyImageGroupResponse>> PresetUpdateAccountAvatar([FromBody] PresetUpdateAccountAvatarRequest request)
        {
            var command = _mapper.Map<PresetUpdateAccountAvatar>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("update-account-birth-date")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateAccountBirthDate([FromBody] UpdateAccountBirthDateRequest request)
        {
            var command = _mapper.Map<UpdateAccountBirthDate>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("update-account-display-name")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateAccountDisplayName([FromBody] UpdateAccountDisplayNameRequest request)
        {
            var command = _mapper.Map<UpdateAccountDisplayName>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("update-account-username")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateAccountUsername([FromBody] UpdateAccountUsernameRequest request)
        {
            var command = _mapper.Map<UpdateAccountUsername>(request);
            await _mediator.Send(command);
            return Ok();
        }
        #endregion

        #region interact
        [HttpPost("follow")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Follow([FromBody] Follow request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("unfollow")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Unfollow([FromBody] Unfollow request)
        {
            await _mediator.Send(request);
            return Ok();
        }
        #endregion
    }
}
