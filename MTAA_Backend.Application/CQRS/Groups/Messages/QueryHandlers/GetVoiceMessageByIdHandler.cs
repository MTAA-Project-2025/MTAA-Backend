using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Groups.Messages.Queries;
using MTAA_Backend.Domain.DTOs.Messages.Responses;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Groups.Messages.QueryHandlers
{
    public class GetVoiceMessageByIdHandler(MTAA_BackendDbContext dbContext,
        IMapper mapper) : IRequestHandler<GetVoiceMessageById, VoiceMessageResponse>
    {
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<VoiceMessageResponse> Handle(GetVoiceMessageById request, CancellationToken cancellationToken)
        {
            var msg = await _dbContext.VoiceMessages.Where(e => e.Id == request.Id)
                .Include(e => e.File)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<VoiceMessageResponse>(msg);
        }
    }
}
