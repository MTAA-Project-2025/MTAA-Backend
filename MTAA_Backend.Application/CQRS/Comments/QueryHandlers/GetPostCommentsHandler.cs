using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Comments.Queries;
using MTAA_Backend.Domain.DTOs.Comments.Responses;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Comments;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.QueryHandlers
{
    public class GetPostCommentsHandler(MTAA_BackendDbContext _dbContext, IMapper _mapper,
        IUserService _userService)
    : IRequestHandler<GetPostComments, ICollection<FullCommentResponse>>
    {
        public async Task<ICollection<FullCommentResponse>> Handle(GetPostComments request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var commentsWithInteraction = await _dbContext.Comments
                .Where(e => e.PostId == request.PostId && e.ParentCommentId == null)
                .OrderByDescending(e => e.DataCreationTime)
                .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                .Take(request.PageParameters.PageSize)
                .Include(e => e.CommentInteractions)
                .Include(e => e.Owner)
                    .ThenInclude(e => e.Avatar)
                        .ThenInclude(e => e.CustomAvatar)
                            .ThenInclude(e => e.Images)
                .Include(e => e.Owner)
                    .ThenInclude(e => e.Avatar)
                        .ThenInclude(e => e.PresetAvatar)
                            .ThenInclude(e => e.Images)
                .Select(c => new
                {
                    Comment = c,
                    InteractionType = c.CommentInteractions
                        .Where(ui => ui.UserId == userId)
                        .Select(ui => (CommentInteractionType?)ui.Type)
                        .FirstOrDefault() ?? CommentInteractionType.None
                })
                .ToListAsync(cancellationToken);

            var result = new List<FullCommentResponse>();

            foreach (var item in commentsWithInteraction)
            {
                var commentResponse = _mapper.Map<FullCommentResponse>(item.Comment);
                commentResponse.Type = item.InteractionType;

                var ownerAvatar = item.Comment.Owner.Avatar;
                if (ownerAvatar != null)
                {
                    if (ownerAvatar.CustomAvatar != null)
                    {
                        commentResponse.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(ownerAvatar.CustomAvatar);
                    }
                    else if (ownerAvatar.PresetAvatar != null)
                    {
                        commentResponse.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(ownerAvatar.PresetAvatar);
                    }
                }

                result.Add(commentResponse);
            }
            return result;
        }
    }
}
