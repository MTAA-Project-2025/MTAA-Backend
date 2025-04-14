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
        public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
        {
            await Guard.Against.NotCommentOwner(id, _dbContext, _localizer, _userService);

            await _mediator.Send(new DeleteComment() { CommentId = id });
            return Ok();
        }

        [HttpPost("get-from-post/{postId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(IEnumerable<FullCommentResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FullCommentResponse>>> GetPostComments([FromRoute] Guid postId, [FromBody] PageParameters pageParameters)
        {
            var comments = await _mediator.Send(new GetPostComments()
            {
                PostId = postId,
                PageParameters = pageParameters
            });
            return Ok(comments);
        }

        [HttpPost("get-from-children/{parentCommentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(IEnumerable<FullCommentResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FullCommentResponse>>> GetChildComments([FromRoute] Guid parentCommentId, [FromBody] PageParameters pageParameters)
        {
            var comments = await _mediator.Send(new GetChildComments()
            {
                ParentCommentId = parentCommentId,
                PageParameters = pageParameters
            });
            return Ok(comments);
        }

        [HttpPost("like/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> LikeComment([FromRoute] Guid commentId)
        {
            await _mediator.Send(new LikeComment() { CommentId = commentId });
            return Ok();
        }

        [HttpPost("dislike/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DislikeComment([FromRoute] Guid commentId)
        {
            await _mediator.Send(new DislikeComment() { CommentId = commentId });
            return Ok();
        }
    }
}
