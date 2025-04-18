﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Users.Relationships.Commands;
using MTAA_Backend.Application.CQRS.Versions.Command;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Versioning;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Users.Relationships.CommandHandlers
{
    public class FollowHandler(IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMediator _mediator) : IRequestHandler<Follow>
    {
        public async Task Handle(Follow request, CancellationToken cancellationToken)
        {
            var currentUserId = _userService.GetCurrentUserId();
            if (currentUserId == null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.Unauthorized);
            }

            if (currentUserId == request.TargetUserId)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.InaccessibleFollowing], HttpStatusCode.BadRequest);
            }

            var targetUserExists = await _dbContext.Users.AnyAsync(u => u.Id == request.TargetUserId, cancellationToken);
            if (!targetUserExists)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.NotFound);
            }

            var relationship = await _dbContext.UserRelationships
                .FirstOrDefaultAsync(r => (r.User1Id == currentUserId && r.User2Id == request.TargetUserId) ||
                                          (r.User1Id == request.TargetUserId && r.User2Id == currentUserId), cancellationToken);

            if (relationship == null)
            {
                relationship = new UserRelationship
                {
                    User1Id = currentUserId,
                    User2Id = request.TargetUserId,
                    IsUser1Following = true,
                    IsUser2Following = false
                };
                _dbContext.UserRelationships.Add(relationship);

                await _mediator.Send(new IncreaseVersion()
                {
                    UserId = request.TargetUserId,
                    VersionItemType = VersionItemType.Followers,
                });
            }
            else
            {
                if (relationship.User1Id == currentUserId)
                {
                    relationship.IsUser1Following = true;
                }
                else
                {
                    relationship.IsUser2Following = true;
                }
                await _mediator.Send(new IncreaseVersion()
                {
                    UserId = currentUserId,
                    VersionItemType = VersionItemType.Friends,
                });
                await _mediator.Send(new IncreaseVersion()
                {
                    UserId = currentUserId,
                    VersionItemType = VersionItemType.Followers,
                });

                await _mediator.Send(new IncreaseVersion()
                {
                    UserId = request.TargetUserId,
                    VersionItemType = VersionItemType.Friends,
                });
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
