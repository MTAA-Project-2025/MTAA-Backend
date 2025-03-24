using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Comments.Queries;
using MTAA_Backend.Domain.DTOs.Comments.Responses;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.QueryHandlers
{
    public class GetPostCommentsHandler(MTAA_BackendDbContext _dbContext,
        IMapper _mapper) : IRequestHandler<GetPostComments, ICollection<FullCommentResponse>>
    {
        public async Task<ICollection<FullCommentResponse>> Handle(GetPostComments request, CancellationToken cancellationToken)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.PostId == request.PostId && c.ParentCommentId == null)
                .OrderByDescending(c => c.DataCreationTime)
                .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                .Take(request.PageParameters.PageSize)
                .Include(c => c.Owner)
                .ToListAsync(cancellationToken);

            return _mapper.Map<ICollection<FullCommentResponse>>(comments);
        }
    }
}
