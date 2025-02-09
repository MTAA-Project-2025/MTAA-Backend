using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MTAA_Backend.Application.CQRS.Users.Identity.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.Queries;
using MTAA_Backend.Domain.DTOs.Users.Identity.Requests;
using MTAA_Backend.Domain.DTOs.Users.Identity.Responses;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Users
{
    public class IdentityController : ApiController
    {
        public IdentityController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [HttpPost]
        [Route("sign-up-start-email-verification")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SignUpStartEmailVerification([FromBody] StartSignUpEmailVerificationRequest request)
        {
            var command = _mapper.Map<StartSignUpEmailVerification>(request);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("sign-up-verify-email")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> SignUpVerifyEmail([FromBody] SignUpVerifyEmailRequest request)
        {
            var command = _mapper.Map<SignUpVerifyEmail>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpPost]
        [Route("sign-up-by-email")]
        [ProducesResponseType(typeof(TokenDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TokenDTO>> SignUpByEmail([FromBody] SignUpByEmailRequest request)
        {
            var command = _mapper.Map<SignUpByEmail>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpPost]
        [Route("log-in")]
        [ProducesResponseType(typeof(TokenDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TokenDTO>> LogIn([FromBody] LogInRequest request)
        {
            var query = _mapper.Map<LogIn>(request);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
