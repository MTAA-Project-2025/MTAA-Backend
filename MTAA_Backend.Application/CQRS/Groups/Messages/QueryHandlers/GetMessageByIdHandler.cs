using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Queries;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.QueryHandlers;
using MTAA_Backend.Application.CQRS.Groups.Messages.Queries;
using MTAA_Backend.Domain.DTOs.Groups.BaseGroups.Responses;
using MTAA_Backend.Domain.DTOs.Messages.Responses;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Messages;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.Messages.QueryHandlers
{
    public class GetMessageByIdHandler(ILogger<GetSimpleGroupByIdHandler> _logger,
        MTAA_BackendDbContext _dbContext,
        IMediator _mediator) : IRequestHandler<GetMessageById, BaseMessageResponse>
    {
        public async Task<BaseMessageResponse> Handle(GetMessageById request, CancellationToken cancellationToken)
        {
            var msg = await _dbContext.BaseMessages.FindAsync(request.Id, cancellationToken);

            if(msg.Type== MessageTypes.FileMessage)
            {
                var resMsg = await _mediator.Send(new GetFileMessageById()
                {
                    Id = request.Id
                });
                return resMsg;
            }
            if (msg.Type == MessageTypes.GifMessage)
            {
                var resMsg = await _mediator.Send(new GetGifMessageById()
                {
                    Id = request.Id
                });
                return resMsg;
            }
            if (msg.Type == MessageTypes.ImagesMessage)
            {
                var resMsg = await _mediator.Send(new GetImagesMessageById()
                {
                    Id = request.Id
                });
                return resMsg;
            }
            if (msg.Type == MessageTypes.TextMesage)
            {
                var resMsg = await _mediator.Send(new GetTextMessageById()
                {
                    Id = request.Id
                });
                return resMsg;
            }
            if (msg.Type == MessageTypes.VoiceMessage)
            {
                var resMsg = await _mediator.Send(new GetVoiceMessageById()
                {
                    Id = request.Id
                });
                return resMsg;
            }
            else
            {
                _logger.LogError($"Message type not supported: {msg.Type}");
                throw new HttpException("MessageTypeNotSupported", HttpStatusCode.BadRequest);
            }
        }
    }
}
