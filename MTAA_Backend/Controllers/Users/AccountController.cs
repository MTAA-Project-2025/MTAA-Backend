using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTAA_Backend.Application.Account.Commands;
using MTAA_Backend.Application.Identity.Commands;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;
using MTAA_Backend.Domain.DTOs.Users.Identity.Requests;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Users
{
    public class AccountController : ApiController
    {
        public AccountController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        #region update
        [HttpPut]
        [Route("custom-update-account-avatar")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> CustomUpdateAccountAvatar([FromForm] CustomUpdateAccountAvatarRequest request)
        {
            var command = _mapper.Map<CustomUpdateAccountAvatar>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route("preset-update-account-avatar")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> PresetUpdateAccountAvatar([FromBody] PresetUpdateAccountAvatarRequest request)
        {
            var command = _mapper.Map<PresetUpdateAccountAvatar>(request);
            await _mediator.Send(command);
            return Ok();
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
