using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Comments.Queries;
using MTAA_Backend.Domain.DTOs.Comments.Responses;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.QueryHandlers
{
    public class GetChildCommentsHandler(MTAA_BackendDbContext _dbContext,
        IMapper _mapper) : IRequestHandler<GetChildComments, ICollection<FullCommentResponse>>
    {
        public async Task<ICollection<FullCommentResponse>> Handle(GetChildComments request, CancellationToken cancellationToken)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.ParentCommentId == request.ParentCommentId)
                .OrderBy(c => c.DataCreationTime)
                .Include(c => c.Owner)
                .ToListAsync(cancellationToken);

            return _mapper.Map<ICollection<FullCommentResponse>>(comments);
        }
    }
}
