using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Api.Guards.Comments;
using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Application.CQRS.Comments.Queries;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Comments.Requests;
using MTAA_Backend.Domain.DTOs.Comments.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Comments
{
    /// <summary>
    /// Controller for managing comments, including creation, editing, deletion, retrieval, and interactions (likes/dislikes).
    /// </summary>
    public class CommentsController : ApiController
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs to commands.</param>
        /// <param name="dbContext">The database context for accessing comment data.</param>
        /// <param name="localizer">The string localizer for error messages.</param>
        /// <param name="userService">The user service for user-related operations.</param>
        public CommentsController(IMediator mediator,
            IMapper mapper,
            MTAA_BackendDbContext dbContext,
            IStringLocalizer<ErrorMessages> localizer,
            IUserService userService) : base(mediator, mapper)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _userService = userService;
        }

        /// <summary>
        /// Adds a new comment to a post or as a reply to another comment.
        /// </summary>
        /// <param name="request">The request containing comment details (e.g., content, post ID, parent comment ID).</param>
        /// <returns>The unique identifier (GUID) of the created comment.</returns>
        /// <response code="200">Returns the GUID of the newly created comment.</response>
        /// <response code="400">If the request is invalid or malformed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost("add")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> AddComment([FromBody] AddCommentRequest request)
        {
            var command = _mapper.Map<AddComment>(request);
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Edits an existing comment's content.
        /// </summary>
        /// <param name="request">The request containing the comment ID and updated content.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The comment was successfully updated.</response>
        /// <response code="400">If the request is invalid or the comment ID is malformed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not the comment owner or lacks the required role.</response>
        /// <response code="404">If the comment does not exist.</response>
        [HttpPut("edit")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditComment([FromBody] EditCommentRequest request)
        {
            await Guard.Against.NotCommentOwner(request.CommentId, _dbContext, _localizer, _userService);

            var command = _mapper.Map<EditComment>(request);
            await _mediator.Send(command);

            return Ok();
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="id">The GUID of the comment to delete.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The comment was successfully deleted.</response>
        /// <response code="400">If the comment ID is not a valid GUID.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not the comment owner or lacks the required role.</response>
        /// <response code="404">If the comment does not exist.</response>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteComment([FromRoute] string id)
        {
            Guid parseId = ParseGuid(id);
            await Guard.Against.NotCommentOwner(parseId, _dbContext, _localizer, _userService);

            await _mediator.Send(new DeleteComment() { CommentId = parseId });
            return Ok();
        }

        /// <summary>
        /// Retrieves paginated comments for a specific post.
        /// </summary>
        /// <param name="postId">The GUID of the post to retrieve comments for.</param>
        /// <param name="pageParameters">Pagination parameters (e.g., page number, page size).</param>
        /// <returns>A collection of comments associated with the post.</returns>
        /// <response code="200">Returns the list of comments.</response>
        /// <response code="400">If the post ID is not a valid GUID or pagination parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost("get-from-post/{postId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(IEnumerable<FullCommentResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FullCommentResponse>>> GetPostComments([FromRoute] string postId, [FromBody] PageParameters pageParameters)
        {
            var comments = await _mediator.Send(new GetPostComments()
            {
                PostId = ParseGuid(postId),
                PageParameters = pageParameters
            });
            return Ok(comments);
        }

        /// <summary>
        /// Retrieves paginated child comments for a specific parent comment.
        /// </summary>
        /// <param name="parentCommentId">The GUID of the parent comment to retrieve child comments for.</param>
        /// <param name="pageParameters">Pagination parameters (e.g., page number, page size).</param>
        /// <returns>A collection of child comments.</returns>
        /// <response code="200">Returns the list of child comments.</response>
        /// <response code="400">If the parent comment ID is not a valid GUID or pagination parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost("get-from-children/{parentCommentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(IEnumerable<FullCommentResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FullCommentResponse>>> GetChildComments([FromRoute] string parentCommentId, [FromBody] PageParameters pageParameters)
        {
            var comments = await _mediator.Send(new GetChildComments()
            {
                ParentCommentId = ParseGuid(parentCommentId),
                PageParameters = pageParameters
            });
            return Ok(comments);
        }

        /// <summary>
        /// Retrieves a single comment by its ID.
        /// </summary>
        /// <param name="id">The GUID of the comment to retrieve.</param>
        /// <returns>The comment details.</returns>
        /// <response code="200">Returns the comment details.</response>
        /// <response code="400">If the comment ID is not a valid GUID.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the comment does not exist.</response>
        [HttpGet("get-by-id/{id}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(FullCommentResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<FullCommentResponse>> GetCommentById([FromRoute] string id)
        {
            var comments = await _mediator.Send(new GetCommentById()
            {
                Id = ParseGuid(id)
            });
            return Ok(comments);
        }

        /// <summary>
        /// Likes a comment by the authenticated user.
        /// </summary>
        /// <param name="commentId">The GUID of the comment to like.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The comment was successfully liked.</response>
        /// <response code="400">If the comment ID is not a valid GUID.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the comment does not exist.</response>
        [HttpPost("like/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> LikeComment([FromRoute] string commentId)
        {
            await _mediator.Send(new LikeComment() { CommentId = ParseGuid(commentId) });
            return Ok();
        }

        /// <summary>
        /// Dislikes a comment by the authenticated user.
        /// </summary>
        /// <param name="commentId">The GUID of the comment to dislike.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The comment was successfully disliked.</response>
        /// <response code="400">If the comment ID is not a valid GUID.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the comment does not exist.</response>
        [HttpPost("dislike/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DislikeComment([FromRoute] string commentId)
        {
            await _mediator.Send(new DislikeComment() { CommentId = ParseGuid(commentId) });
            return Ok();
        }

        /// <summary>
        /// Removes the authenticated user's like or dislike from a comment.
        /// </summary>
        /// <param name="commentId">The GUID of the comment to reset interaction for.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The interaction was successfully removed.</response>
        /// <response code="400">If the comment ID is not a valid GUID.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the comment does not exist.</response>
        [HttpPost("set-interaction-to-none/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SetCommentInteractionToNone([FromRoute] string commentId)
        {
            await _mediator.Send(new SetCommentInteractionToNone() { CommentId = ParseGuid(commentId) });
            return Ok();
        }

        /// <summary>
        /// Parses a string into a GUID, throwing an exception if invalid.
        /// </summary>
        /// <param name="id">The string to parse as a GUID.</param>
        /// <returns>The parsed GUID.</returns>
        /// <exception cref="HttpException">Thrown when the ID is not a valid GUID.</exception>
        private Guid ParseGuid(string id)
        {
            Guid parsedId;
            var parseRes = Guid.TryParse(id, out parsedId);
            if (!parseRes)
            {
                throw new HttpException("Id is not in the correct format of GUID", HttpStatusCode.BadRequest);
            }
            return parsedId;
        }
    }
}
