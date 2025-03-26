using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Comments.Queries;
using MTAA_Backend.Domain.DTOs.Comments.Responses;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.QueryHandlers
{
    public class GetChildCommentsHandler(MTAA_BackendDbContext _dbContext, IMapper _mapper) : IRequestHandler<GetChildComments, ICollection<FullCommentResponse>>
    {
        public async Task<ICollection<FullCommentResponse>> Handle(GetChildComments request, CancellationToken cancellationToken)
        {
            var comments = await _dbContext.Comments
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
                .ToListAsync(cancellationToken);

            var result = new List<FullCommentResponse>();

            for (int i = 0; i < comments.Count; i++)
            {
                var commentResponse = _mapper.Map<FullCommentResponse>(comments[i]);
                var ownerAvatar = comments[i].Owner.Avatar;

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
