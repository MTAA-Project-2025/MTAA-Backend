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
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using Nest;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Posts
{
    public class PostsController : ApiController
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IUserService _userService;
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
