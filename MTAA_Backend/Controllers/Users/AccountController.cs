using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Application.CQRS.Users.Relationships.Commands;
using MTAA_Backend.Application.CQRS.Users.Relationships.Queries;
using MTAA_Backend.Application.CQRS.Versions.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.DTOs.Users.Identity.Other;
using MTAA_Backend.Domain.DTOs.Versioning.Responses;
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
        [Route("full-account")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(UserFullAccountResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserFullAccountResponse>> GetUserFullAccount()
        {
            var query = new GetUserFullAccount();
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpPost("get-followers")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(ICollection<PublicBaseAccountResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFollowers([FromBody] GlobalSearchRequest request)
        {
            var query = new GetFollowers()
            {
                FilterStr = request.FilterStr,
                PageParameters = request.PageParameters
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("get-friends")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(ICollection<PublicBaseAccountResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFriends([FromBody] GlobalSearchRequest request)
        {
            var query = new GetFriends()
            {
                FilterStr = request.FilterStr,
                PageParameters = request.PageParameters
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("all-versions")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(ICollection<VersionItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllVersions()
        {
            var result = await _mediator.Send(new GetAllVersionItems());
            return Ok(result);
        }
        #endregion

        [HttpPost("save-firebase-token")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SaveFirebaseToken([FromBody] FirebaseTokenRequest request)
        {
            await _mediator.Send(new SaveFirebaseToken()
            {
                Token = request.Token
            });
            return Ok();
        }

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
    }
}
