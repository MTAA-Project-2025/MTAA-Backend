using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Groups.Messages.Queries;
using MTAA_Backend.Domain.DTOs.Messages.Responses;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Groups.Messages.QueryHandlers
{
    public class GetGifMessageByIdHandler(MTAA_BackendDbContext _dbContext,
        IMapper _mapper) : IRequestHandler<GetGifMessageById, GifMessageResponse>
    {
        public async Task<GifMessageResponse> Handle(GetGifMessageById request, CancellationToken cancellationToken)
        {
            var msg = await _dbContext.GifMessages.Where(e => e.Id == request.Id)
                .Include(e => e.File)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<GifMessageResponse>(msg);
        }
    }
}
