using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Groups.Messages.Queries;
using MTAA_Backend.Domain.DTOs.Messages.Responses;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Groups.Messages.QueryHandlers
{
    public class GetTextMessageByIdHandler(MTAA_BackendDbContext dbContext,
        IMapper mapper) : IRequestHandler<GetTextMessageById, TextMessageResponse>
    {
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<TextMessageResponse> Handle(GetTextMessageById request, CancellationToken cancellationToken)
        {
            var msg = await _dbContext.TextMessages.FindAsync(request.Id, cancellationToken);

            return _mapper.Map<TextMessageResponse>(msg);
        }
    }
}
