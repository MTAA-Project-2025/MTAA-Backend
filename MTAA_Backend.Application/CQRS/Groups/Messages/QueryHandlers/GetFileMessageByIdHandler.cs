using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Queries;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.QueryHandlers;
using MTAA_Backend.Application.CQRS.Groups.Messages.Queries;
using MTAA_Backend.Domain.DTOs.Groups.BaseGroups.Responses;
using MTAA_Backend.Domain.DTOs.Messages.Responses;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Groups.Messages.QueryHandlers
{
    public class GetFileMessageByIdHandler(MTAA_BackendDbContext _dbContext,
        IMapper _mapper) : IRequestHandler<GetFileMessageById, FileMessageResponse>
    {
        public async Task<FileMessageResponse> Handle(GetFileMessageById request, CancellationToken cancellationToken)
        {
            var msg = await _dbContext.FileMessages.Where(e => e.Id == request.Id)
                .Include(e => e.File)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<FileMessageResponse>(msg);
        }
    }
}
