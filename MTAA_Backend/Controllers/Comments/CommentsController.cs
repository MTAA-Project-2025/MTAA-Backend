using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Application.CQRS.Comments.Queries;
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
        public async Task<IActionResult> AddComment([FromBody] AddComment request)
        {
            var id = await _mediator.Send(request);
            return Ok(id);
        }

        [HttpPut("edit")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> EditComment([FromBody] EditComment request)
        {
            var comment = await _dbContext.Comments.Include(c => c.Post).FirstOrDefaultAsync(c => c.Id == request.CommentId);
            if (comment == null || (comment.OwnerId != _userService.GetCurrentUserId() && comment.Post.OwnerId != _userService.GetCurrentUserId()))
            {
                return Forbid(_localizer["You are not authorized to edit this comment."]);
            }

            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _dbContext.Comments.Include(c => c.Post).FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null || (comment.OwnerId != _userService.GetCurrentUserId() && comment.Post.OwnerId != _userService.GetCurrentUserId()))
            {
                return Forbid(_localizer["You are not authorized to delete this comment."]);
            }

            await _mediator.Send(new DeleteComment { CommentId = id });
            return Ok();
        }

        [HttpGet("post/{postId}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> GetPostComments(Guid postId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var comments = await _mediator.Send(new GetPostComments
            {
                PostId = postId,
                PageParameters = new PageParameters { PageNumber = pageNumber, PageSize = pageSize }
            });
            return Ok(comments);
        }

        [HttpGet("children/{parentCommentId}")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> GetChildComments(Guid parentCommentId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var comments = await _mediator.Send(new GetChildComments
            {
                ParentCommentId = parentCommentId,
                PageParameters = new PageParameters { PageNumber = pageNumber, PageSize = pageSize }
            });
            return Ok(comments);
        }
    }
}
