using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Users.Relationships.Commands;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Users.Relationships.CommandHandlers
{
    public class UnfollowHandler(ILogger<UnfollowHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService) : IRequestHandler<Unfollow>
    {
        public async Task Handle(Unfollow request, CancellationToken cancellationToken)
        {
            var currentUserId = _userService.GetCurrentUserId();
            if (currentUserId == null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotAuthorized], HttpStatusCode.Unauthorized);
            }

            var relationship = await _dbContext.UserRelationships
                .FirstOrDefaultAsync(r => (r.User1Id == currentUserId && r.User2Id == request.TargetUserId) ||
                                          (r.User1Id == request.TargetUserId && r.User2Id == currentUserId), cancellationToken);

            if (relationship == null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFollowed], HttpStatusCode.BadRequest);
            }

            if (relationship.User1Id == currentUserId)
            {
                relationship.IsUser1Following = false;
            }
            else
            {
                relationship.IsUser2Following = false;
            }

            if (!relationship.IsUser1Following && !relationship.IsUser2Following)
            {
                _dbContext.UserRelationships.Remove(relationship);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
