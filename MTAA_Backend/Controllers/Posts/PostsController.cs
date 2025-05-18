using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Api.Guards.Posts;
using MTAA_Backend.Application.CQRS.Locations.Commands;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Application.CQRS.Posts.QueryHandlers;
using MTAA_Backend.Domain.DTOs.Posts.Requests;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using Nest;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Posts
{
    /// <summary>
    /// Controller for managing posts, including creation, updating, deletion, retrieval, and interactions (likes).
    /// </summary>
    public class PostsController : ApiController
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs to commands.</param>
        /// <param name="dbContext">The database context for accessing post data.</param>
        /// <param name="localizer">The string localizer for error messages.</param>
        /// <param name="userService">The user service for user-related operations.</param>
        public PostsController(IMediator mediator,
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
        /// Creates a new post, optionally with location data.
        /// </summary>
        /// <param name="request">The request containing post details (e.g., content, location).</param>
        /// <returns>The unique identifier (GUID) of the created post.</returns>
        /// <response code="200">Returns the GUID of the newly created post.</response>
        /// <response code="400">If the request is invalid or malformed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> AddPost([FromForm] AddPostRequest request)
        {
            var command = _mapper.Map<AddPost>(request);
            var res = await _mediator.Send(command);

            if (request.Location != null)
            {
                await _mediator.Send(new AddPostLocation()
                {
                    PostId = res,
                    Latitude=request.Location.Latitude,
                    EventTime=request.Location.EventTime,
                    Longitude=request.Location.Longitude
                });
            }
            return Ok(res);
        }

        /// <summary>
        /// Updates an existing post, optionally with new location data.
        /// </summary>
        /// <param name="request">The request containing the post ID and updated details (e.g., content, location).</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The post was successfully updated.</response>
        /// <response code="400">If the request is invalid or the post ID is malformed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not the post owner or lacks the required role.</response>
        /// <response code="404">If the post does not exist.</response>
        [HttpPut]
        [Authorize(Roles = UserRoles.User)]
        [Route("update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdatePost([FromForm] UpdatePostRequest request)
        {
            await Guard.Against.NotPostOwner(request.Id, _dbContext, _localizer, _userService);

            var command = _mapper.Map<UpdatePost>(request);
            await _mediator.Send(command);

            if (request.Location != null)
            {
                await _mediator.Send(new UpdatePostLocation()
                {
                    EventTime = request.Location.EventTime,
                    Latitude = request.Location.Latitude,
                    Longitude = request.Location.Longitude,
                    PostId = request.Id
                });
            }

            return Ok();
        }

        /// <summary>
        /// Retrieves paginated recommended posts for the authenticated user.
        /// </summary>
        /// <param name="request">Pagination parameters (e.g., page number, page size).</param>
        /// <returns>A collection of recommended posts.</returns>
        /// <response code="200">Returns the list of recommended posts.</response>
        /// <response code="400">If the pagination parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-recommendations")]
        [ProducesResponseType(typeof(ICollection<FullPostResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<FullPostResponse>>> GetRecommendedPosts([FromBody] PageParameters request)
        {
            var res = await _mediator.Send(new GetRecommendedPosts()
            {
                PageParameters = request
            });
            return Ok(res);
        }

        /// <summary>
        /// Retrieves paginated global posts based on search criteria.
        /// </summary>
        /// <param name="request">The request containing search parameters (e.g., keywords, filters).</param>
        /// <returns>A collection of global posts.</returns>
        /// <response code="200">Returns the list of global posts.</response>
        /// <response code="400">If the search parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-global")]
        [ProducesResponseType(typeof(ICollection<FullPostResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<FullPostResponse>>> GetGlobalPosts([FromBody] GlobalSearchRequest request)
        {
            var command = _mapper.Map<GetGlobalPosts>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        /// <summary>
        /// Retrieves paginated posts liked by the authenticated user.
        /// </summary>
        /// <param name="pageParameters">Pagination parameters (e.g., page number, page size).</param>
        /// <returns>A collection of liked posts.</returns>
        /// <response code="200">Returns the list of liked posts.</response>
        /// <response code="400">If the pagination parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-liked")]
        [ProducesResponseType(typeof(ICollection<FullPostResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<FullPostResponse>>> GetLikedPosts([FromBody] PageParameters pageParameters)
        {
            var res = await _mediator.Send(new GetLikedPosts()
            {
                PageParameters = pageParameters,
            });
            return Ok(res);
        }

        /// <summary>
        /// Retrieves a single post by its ID.
        /// </summary>
        /// <param name="id">The GUID of the post to retrieve.</param>
        /// <returns>The details of the post.</returns>
        /// <response code="200">Returns the post details.</response>
        /// <response code="400">If the post ID is not a valid GUID.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the post does not exist.</response>
        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-by-id/{id}")]
        [ProducesResponseType(typeof(FullPostResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<FullPostResponse>> GetFullPostById([FromRoute] string id)
        {
            Guid parsedId;
            var parseRes = Guid.TryParse(id, out parsedId);
            if (!parseRes)
            {
                throw new HttpException("Id is not in the correct format of GUID", HttpStatusCode.BadRequest);
            }

            var res = await _mediator.Send(new GetFullPostById()
            {
                Id = parsedId
            });
            return Ok(res);
        }

        /// <summary>
        /// Retrieves paginated posts created by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose posts to retrieve.</param>
        /// <param name="pageParameters">Pagination parameters (e.g., page number, page size).</param>
        /// <returns>A collection of posts created by the user.</returns>
        /// <response code="200">Returns the list of user posts.</response>
        /// <response code="400">If the pagination parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the user does not exist.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-from-account/{userId}")]
        [ProducesResponseType(typeof(ICollection<SimplePostResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<SimplePostResponse>>> GetAccountPosts([FromRoute] string userId, [FromBody] PageParameters pageParameters)
        {
            var res = await _mediator.Send(new GetAccountPosts()
            {
                UserId = userId,
                PageParameters = pageParameters
            });
            return Ok(res);
        }

        /// <summary>
        /// Retrieves paginated scheduled posts for the authenticated user.
        /// </summary>
        /// <param name="pageParameters">Pagination parameters (e.g., page number, page size).</param>
        /// <returns>A collection of scheduled posts.</returns>
        /// <response code="200">Returns the list of scheduled posts.</response>
        /// <response code="400">If the pagination parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-scheduled-posts")]
        [ProducesResponseType(typeof(ICollection<SchedulePostResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<SchedulePostResponse>>> GetSchedulePosts([FromBody] PageParameters pageParameters)
        {
            var res = await _mediator.Send(new GetSchedulePosts()
            {
                PageParameters = pageParameters
            });
            return Ok(res);
        }

        /// <summary>
        /// Retrieves a single scheduled post by its ID.
        /// </summary>
        /// <param name="id">The GUID of the scheduled post to retrieve.</param>
        /// <returns>The details of the scheduled post, or null if not found.</returns>
        /// <response code="200">Returns the scheduled post details or null.</response>
        /// <response code="400">If the post ID is not a valid GUID.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the scheduled post does not exist.</response>
        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-scheduled-post/{id}")]
        [ProducesResponseType(typeof(SchedulePostResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SchedulePostResponse?>> GetSchedulePosts([FromRoute] string id)
        {
            Guid parsedId;
            var parseRes = Guid.TryParse(id, out parsedId);
            if (!parseRes)
            {
                throw new HttpException("Id is not in the correct format of GUID", HttpStatusCode.BadRequest);
            }

            var res = await _mediator.Send(new GetSchedulePostById()
            {
                Id = parsedId
            });
            return Ok(res);
        }

        /// <summary>
        /// Retrieves paginated version items for posts (e.g., post revisions or drafts).
        /// </summary>
        /// <param name="pageParameters">Pagination parameters (e.g., page number, page size).</param>
        /// <returns>A collection of post version items.</returns>
        /// <response code="200">Returns the list of post version items.</response>
        /// <response code="400">If the pagination parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-post-version-items")]
        [ProducesResponseType(typeof(ICollection<VersionPostItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<VersionPostItemResponse>>> GetPostVersionItems([FromBody] PageParameters pageParameters)
        {
            var res = await _mediator.Send(new GetPostVersionItems()
            {
                PageParameters = pageParameters
            });
            return Ok(res);
        }

        /// <summary>
        /// Deletes a post by its ID.
        /// </summary>
        /// <param name="id">The GUID of the post to delete.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The post was successfully deleted.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not the post owner or lacks the required role.</response>
        /// <response code="404">If the post does not exist.</response>
        [HttpDelete]
        [Authorize(Roles = UserRoles.User)]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeletePost([FromRoute] Guid id)
        {
            await Guard.Against.NotPostOwner(id, _dbContext, _localizer, _userService);

            await _mediator.Send(new DeletePost()
            {
                Id = id
            });
            return Ok();
        }

        /// <summary>
        /// Adds a like to a post by the authenticated user.
        /// </summary>
        /// <param name="postId">The GUID of the post to like.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The post was successfully liked.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the post does not exist.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("add-like/{postId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> AddLike([FromRoute] Guid postId)
        {
            await _mediator.Send(new AddPostLike()
            {
                Id = postId
            });

            return Ok();
        }

        /// <summary>
        /// Removes a like from a post by the authenticated user.
        /// </summary>
        /// <param name="postId">The GUID of the post to unlike.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="200">The like was successfully removed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the post does not exist or was not liked by the user.</response>
        [HttpDelete]
        [Authorize(Roles = UserRoles.User)]
        [Route("remove-like/{postId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> RemoveLike([FromRoute] Guid postId)
        {
            await _mediator.Send(new RemovePostLike()
            {
                Id = postId
            });

            return Ok();
        }
    }
}
