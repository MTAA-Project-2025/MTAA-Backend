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
    /// <summary>
    /// Controller for managing user account operations, including profile retrieval, updates, relationships, and Firebase token storage.
    /// </summary>
    public class AccountController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs to commands.</param>
        public AccountController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        #region get

        /// <summary>
        /// Retrieves the full account details of the authenticated user.
        /// </summary>
        /// <returns>The full account details of the user.</returns>
        /// <response code="200">Returns the user's full account details.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
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

        /// <summary>
        /// Retrieves paginated followers of the authenticated user, optionally filtered by a search string.
        /// </summary>
        /// <param name="request">The request containing pagination parameters and an optional filter string.</param>
        /// <returns>A collection of follower accounts.</returns>
        /// <response code="200">Returns the list of followers.</response>
        /// <response code="400">If the pagination parameters or filter string are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
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

        /// <summary>
        /// Retrieves paginated friends of the authenticated user, optionally filtered by a search string.
        /// </summary>
        /// <param name="request">The request containing pagination parameters and an optional filter string.</param>
        /// <returns>A collection of friend accounts.</returns>
        /// <response code="200">Returns the list of friends.</response>
        /// <response code="400">If the pagination parameters or filter string are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
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

        /// <summary>
        /// Retrieves all version items associated with the authenticated user's account.
        /// </summary>
        /// <returns>A collection of version items.</returns>
        /// <response code="200">Returns the list of version items.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpGet("all-versions")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(ICollection<VersionItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllVersions()
        {
            var result = await _mediator.Send(new GetAllVersionItems());
            return Ok(result);
        }
        #endregion

        /// <summary>
        /// Saves a Firebase token for push notifications for the authenticated user.
        /// </summary>
        /// <param name="request">The request containing the Firebase token.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The Firebase token was successfully saved.</response>
        /// <response code="400">If the token is invalid or malformed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
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

        /// <summary>
        /// Updates the authenticated user's avatar with a custom image.
        /// </summary>
        /// <param name="request">The request containing the custom avatar image data.</param>
        /// <returns>The updated avatar image details.</returns>
        /// <response code="200">Returns the updated avatar image details.</response>
        /// <response code="400">If the request or image data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
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

        /// <summary>
        /// Updates the authenticated user's avatar with a preset image.
        /// </summary>
        /// <param name="request">The request containing the preset image ID.</param>
        /// <returns>The updated avatar image details.</returns>
        /// <response code="200">Returns the updated avatar image details.</response>
        /// <response code="400">If the preset image ID is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the preset image does not exist.</response>
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

        /// <summary>
        /// Updates the authenticated user's birth date.
        /// </summary>
        /// <param name="request">The request containing the new birth date.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The birth date was successfully updated.</response>
        /// <response code="400">If the birth date is invalid or malformed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
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

        /// <summary>
        /// Updates the authenticated user's display name.
        /// </summary>
        /// <param name="request">The request containing the new display name.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The display name was successfully updated.</response>
        /// <response code="400">If the display name is invalid or already in use.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
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

        /// <summary>
        /// Updates the authenticated user's username.
        /// </summary>
        /// <param name="request">The request containing the new username.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The username was successfully updated.</response>
        /// <response code="400">If the username is invalid or already in use.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
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
