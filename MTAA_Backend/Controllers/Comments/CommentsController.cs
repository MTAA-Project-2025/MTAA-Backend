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
    public class CommentsController : ApiController
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IUserService _userService;
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

        [HttpPost("add")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> AddComment([FromBody] AddCommentRequest request)
        {
            var command = _mapper.Map<AddComment>(request);
            var res = await _mediator.Send(command);

            return Ok(res);
        }

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

        [HttpPost("like/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> LikeComment([FromRoute] string commentId)
        {
            await _mediator.Send(new LikeComment() { CommentId = ParseGuid(commentId) });
            return Ok();
        }

        [HttpPost("dislike/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DislikeComment([FromRoute] string commentId)
        {
            await _mediator.Send(new DislikeComment() { CommentId = ParseGuid(commentId) });
            return Ok();
        }

        [HttpPost("set-interaction-to-none/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SetCommentInteractionToNone([FromRoute] string commentId)
        {
            await _mediator.Send(new SetCommentInteractionToNone() { CommentId = ParseGuid(commentId) });
            return Ok();
        }

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
