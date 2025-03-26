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
using MTAA_Backend.Domain.DTOs.Comments.Requests;
using MTAA_Backend.Domain.DTOs.Comments.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Comments
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public CommentsController(IMediator mediator, MTAA_BackendDbContext dbContext, IUserService userService, IStringLocalizer<ErrorMessages> localizer)
        {
            _mediator = mediator;
            _dbContext = dbContext;
            _userService = userService;
            _localizer = localizer;
        }

        [HttpPost("add")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> AddComment([FromBody] AddCommentRequest request)
        {
            var id = await _mediator.Send(request);
            return Ok(id);
        }

        [HttpPut("edit")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditComment([FromBody] EditCommentRequest request)
        {
            await Guard.Against.NotCommentOwner(request.CommentId, _dbContext, _userService, _localizer);

            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentRequest request)
        {
            await Guard.Against.NotCommentOwner(request.CommentId, _dbContext, _userService, _localizer);

            await _mediator.Send(request);
            return Ok();
        }

        [HttpGet("post/{postId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(IEnumerable<FullCommentResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FullCommentResponse>>> GetPostComments([FromBody] GetPostCommentsRequest request)
        {
            var comments = await _mediator.Send(request);
            return Ok(comments);
        }

        [HttpGet("children/{parentCommentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(typeof(IEnumerable<FullCommentResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FullCommentResponse>>> GetChildComments([FromBody] GetChildCommentsRequest request)
        {
            var comments = await _mediator.Send(request);
            return Ok(comments);
        }

        [HttpPost("like/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> LikeComment([FromBody] LikeCommentRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("dislike/{commentId}")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DislikeComment([FromBody] DislikeCommentRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
