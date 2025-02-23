using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.CQRS.Groups.BaseGroups.Queries;
using MTAA_Backend.Application.Services;
using MTAA_Backend.Domain.DTOs.Groups.BaseGroups.Responses;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Groups;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.BaseGroups.QueryHandlers
{
    public class GetSimpleGroupByIdHandler(ILogger<GetSimpleGroupByIdHandler> logger,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext,
        IMapper mapper,
        IUserService userService) : IRequestHandler<GetSimpleGroupById, SimpleBaseGroupResponse>
    {
        private readonly ILogger _logger = logger;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        private readonly IUserService _userService = userService;

        public async Task<SimpleBaseGroupResponse> Handle(GetSimpleGroupById request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var group = await _dbContext.BaseGroups.FindAsync(request.Id, cancellationToken);
            if (group == null)
            {
                _logger.LogError("Group with id {0} not found", request.Id);
                throw new HttpException(_localizer[ErrorMessagesPatterns.GroupNotFound], HttpStatusCode.NotFound);
            }

            SimpleBaseGroupResponse response = _mapper.Map<SimpleBaseGroupResponse>(group);
            MyImageGroup image = null;
            if (group.Type == GroupTypes.Chat)
            {
                var chat = await _dbContext.ContactChats.Where(e => e.Id == request.Id)
                                                        .Include(e => e.Participants)
                                                        .ThenInclude(e => e.Avatar)
                                                        .ThenInclude(e => e.PresetAvatar)
                                                        .ThenInclude(e => e.Images)
                                                        .Include(e => e.Participants)
                                                        .ThenInclude(e => e.Avatar)
                                                        .ThenInclude(e => e.CustomAvatar)
                                                        .ThenInclude(e => e.Images)
                                                        .FirstOrDefaultAsync(cancellationToken);

                var user = chat?.Participants?.FirstOrDefault(e => e.Id != userId);

                if (user != null)
                {
                    if (user.Avatar?.CustomAvatar != null) image = user.Avatar.CustomAvatar;
                    else image = user.Avatar.PresetAvatar;
                    response.Title = user.DisplayName;
                }
            }
            else if (group.Type == GroupTypes.Channel)
            {
                var channel = await _dbContext.Channels.Where(e => e.Id == request.Id)
                                                       .Include(e => e.Image)
                                                            .ThenInclude(e=>e.Images)
                                                       .FirstOrDefaultAsync(cancellationToken);
                image = channel?.Image;
                response.Title = channel?.DisplayName;
            }
            if (image != null)
            {
                response.Image = _mapper.Map<MyImageGroupResponse>(image);
                response.Image.Images = response.Image.Images.OrderBy(e => e.Width).ToList();
            }

            return response;
        }
    }
}
