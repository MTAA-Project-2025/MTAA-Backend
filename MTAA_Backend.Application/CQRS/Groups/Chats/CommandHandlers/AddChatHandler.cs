﻿using AutoMapper;
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
    public class AddChatHandler(ILogger<AddChatHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService) : IRequestHandler<AddChat, Guid>
    {
        public async Task<Guid> Handle(AddChat request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogError("User not authorized");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.NotFound);
            }

            var newChat = new ContactChat()
            {
                Visibility = GroupVisibilityTypes.Invisible
            };

            var contactExists = await _dbContext.Users.AnyAsync(u => u.Id == request.ContactId, cancellationToken);
            if (!contactExists)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.NotFound);
            }

            var chat = await _dbContext.ContactChats.Where(e => e.Participants.Any(e => e.Id == userId) &&
                e.Participants.Any(e => e.Id == request.ContactId)).FirstOrDefaultAsync(cancellationToken);

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
