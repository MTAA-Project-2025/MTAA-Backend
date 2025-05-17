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
    /// <summary>
    /// Controller for managing user identity operations, including registration, email verification, and login.
    /// </summary>
    public class IdentityController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs to commands.</param>
        public IdentityController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        /// <summary>
        /// Initiates the email verification process for user registration.
        /// </summary>
        /// <param name="request">The request containing the email address to verify.</param>
        /// <returns>An empty response indicating the verification process has started.</returns>
        /// <response code="200">The email verification process was successfully initiated.</response>
        /// <response code="400">If the email address is invalid or already in use.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [HttpPost]
        [Route("sign-up-start-email-verification")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> SignUpStartEmailVerification([FromBody] StartSignUpEmailVerificationRequest request)
        {
            var command = _mapper.Map<StartSignUpEmailVerification>(request);
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Verifies the email address using the provided verification code.
        /// </summary>
        /// <param name="request">The request containing the email address and verification code.</param>
        /// <returns>A boolean indicating whether the email was successfully verified.</returns>
        /// <response code="200">Returns true if the email was verified, false otherwise.</response>
        /// <response code="400">If the email or verification code is invalid.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [HttpPost]
        [Route("sign-up-verify-email")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> SignUpVerifyEmail([FromBody] SignUpVerifyEmailRequest request)
        {
            var command = _mapper.Map<SignUpVerifyEmail>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        /// <summary>
        /// Registers a new user using their email address and verified credentials.
        /// </summary>
        /// <param name="request">The request containing user registration details (e.g., email, password).</param>
        /// <returns>A token DTO containing authentication details for the registered user.</returns>
        /// <response code="200">Returns the authentication token for the registered user.</response>
        /// <response code="400">If the request is invalid, the email is not verified, or credentials are malformed.</response>
        /// <response code="500">If an unexpected server error occurs during registration.</response>
        [HttpPost]
        [Route("sign-up-by-email")]
        [ProducesResponseType(typeof(TokenDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TokenDTO>> SignUpByEmail([FromBody] SignUpByEmailRequest request)
        {
            var command = _mapper.Map<SignUpByEmail>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        /// <summary>
        /// Authenticates a user and returns an authentication token.
        /// </summary>
        /// <param name="request">The request containing login credentials (e.g., email, password).</param>
        /// <returns>A token DTO containing authentication details for the logged-in user.</returns>
        /// <response code="200">Returns the authentication token for the user.</response>
        /// <response code="400">If the credentials are invalid or malformed.</response>
        /// <response code="401">If the login attempt fails due to incorrect credentials.</response>
        /// <response code="500">If an unexpected server error occurs during login.</response>
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
