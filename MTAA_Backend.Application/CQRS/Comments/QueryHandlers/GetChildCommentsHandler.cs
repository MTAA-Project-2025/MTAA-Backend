using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Comments.Queries;
using MTAA_Backend.Domain.DTOs.Comments.Responses;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Resources.Comments;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.QueryHandlers
{
    public class GetChildCommentsHandler(MTAA_BackendDbContext _dbContext, IMapper _mapper)
    : IRequestHandler<GetChildComments, ICollection<FullCommentResponse>>
    {
        public async Task<ICollection<FullCommentResponse>> Handle(GetChildComments request, CancellationToken cancellationToken)
        {
            var commentsWithInteraction = await _dbContext.Comments
                .Where(e => e.ParentCommentId == request.ParentCommentId)
                .OrderBy(e => e.DataCreationTime)
                .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                .Take(request.PageParameters.PageSize)
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
                    .Where(ui => ui.UserId == request.UserId)
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
