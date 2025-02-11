using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.Chats.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Groups;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Groups.Chats.CommandHandlers
{
    public class AddChatHandler(ILogger<SignUpByEmailHandler> logger,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext,
        IUserService userService) : IRequestHandler<AddChat, Guid>
    {
        private readonly ILogger _logger = logger;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IUserService _userService = userService;

        public async Task<Guid> Handle(AddChat request, CancellationToken cancellationToken)
        {
            var visibilities = GroupVisibilityTypes.GetAll();
            if (!visibilities.Contains(request.Visibility))
            {
                _logger.LogError($"Visibility not found: {request.Visibility}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.GroupVisibilityTypeDontExist], HttpStatusCode.BadRequest);
            }

            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.NotFound);
            }

            var newChat = new ContactChat()
            {
                IdentificationName = request.IdentificationName,
                Visibility = request.Visibility,
                UserId = userId
            };

            var chat = await _dbContext.ContactChats.Where(e => e.IdentificationName == request.IdentificationName)
                                                   .FirstOrDefaultAsync(cancellationToken);

            if (chat != null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.ChatAlreadyExist], HttpStatusCode.BadRequest);
            }

            _dbContext.ContactChats.Add(newChat);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return newChat.Id;
        }
    }
}
