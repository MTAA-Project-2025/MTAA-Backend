using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
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
        [ProducesResponseType(typeof(ICollection<PublicFullAccountResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PublicFullAccountResponse>> PublicGetFullAccount([FromRoute] string userId)
        {
            var query = new PublicGetFullAccount()
            {
                UserId = userId
            };
            var res = await _mediator.Send(query);
            return Ok(res);
        }
        #endregion

        #region update
        [HttpPut]
        [Route("custom-update-account-avatar")]
        [ProducesResponseType(typeof(ICollection<MyImageGroupResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MyImageGroupResponse>> CustomUpdateAccountAvatar([FromForm] CustomUpdateAccountAvatarRequest request)
        {
            var command = _mapper.Map<CustomUpdateAccountAvatar>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpPut]
        [Route("preset-update-account-avatar")]
        [ProducesResponseType(typeof(ICollection<MyImageGroupResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MyImageGroupResponse>> PresetUpdateAccountAvatar([FromBody] PresetUpdateAccountAvatarRequest request)
        {
            var command = _mapper.Map<PresetUpdateAccountAvatar>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpPut]
        [Route("update-account-birth-date")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateAccountBirthDate([FromBody] UpdateAccountBirthDateRequest request)
        {
            var command = _mapper.Map<UpdateAccountBirthDate>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route("update-account-display-name")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateAccountDisplayName([FromBody] UpdateAccountDisplayNameRequest request)
        {
            var command = _mapper.Map<UpdateAccountDisplayName>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
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
