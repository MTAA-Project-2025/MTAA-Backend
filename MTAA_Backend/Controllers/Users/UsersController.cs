using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Application.CQRS.Users.Relationships.Commands;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Resources.Customers;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Users
{
    /// <summary>
    /// Controller for managing user-related operations, including public account retrieval, global user search, and follow/unfollow actions.
    /// </summary>
    public class UsersController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs to commands.</param>
        public UsersController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        /// <summary>
        /// Retrieves the public full account details of a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose public account details to retrieve.</param>
        /// <returns>The public full account details of the user.</returns>
        /// <response code="200">Returns the public full account details.</response>
        /// <response code="400">If the user ID is invalid or malformed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the user does not exist.</response>
        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
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

        /// <summary>
        /// Retrieves paginated users based on global search criteria.
        /// </summary>
        /// <param name="request">The request containing search parameters (e.g., filter string, pagination).</param>
        /// <returns>A collection of public base account details for matching users.</returns>
        /// <response code="200">Returns the list of matching users.</response>
        /// <response code="400">If the search parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-global")]
        [ProducesResponseType(typeof(ICollection<PublicBaseAccountResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<PublicBaseAccountResponse>>> GetGlobalUsers([FromBody] GlobalSearchRequest request)
        {
            var command = _mapper.Map<GetGlobalUsers>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        /// <summary>
        /// Allows the authenticated user to follow another user.
        /// </summary>
        /// <param name="request">The request containing the ID of the user to follow.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The follow action was successful.</response>
        /// <response code="400">If the request is invalid or the user ID is malformed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role or cannot follow the target user.</response>
        /// <response code="404">If the target user does not exist.</response>
        [HttpPost("follow")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Follow([FromBody] Follow request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        /// <summary>
        /// Allows the authenticated user to unfollow another user.
        /// </summary>
        /// <param name="request">The request containing the ID of the user to unfollow.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The unfollow action was successful.</response>
        /// <response code="400">If the request is invalid or the user ID is malformed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role or is not following the target user.</response>
        /// <response code="404">If the target user does not exist.</response>
        [HttpPost("unfollow")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Unfollow([FromBody] Unfollow request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
