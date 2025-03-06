using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Groups.Messages.Queries;
using MTAA_Backend.Domain.DTOs.Messages.Responses;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Groups.Messages.QueryHandlers
{
    public class GetImagesMessageByIdHandler(MTAA_BackendDbContext dbContext,
        IMapper mapper) : IRequestHandler<GetImagesMessageById, ImagesMessageResponse>
    {
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<ImagesMessageResponse> Handle(GetImagesMessageById request, CancellationToken cancellationToken)
        {
            var msg = await _dbContext.ImagesMessages.Where(e => e.Id == request.Id)
                .Include(e => e.Images)
                    .ThenInclude(e => e.Images)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<ImagesMessageResponse>(msg);
        }
    }
}
