using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Application.Services.RecommendationSystem.RecommendationFeedServices;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Application.CQRS.Posts.CommandHandlers
{
    public class RemovePostLikeHandler(ILogger<RemovePostLikeHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService) : IRequestHandler<RemovePostLike>
    {
        public async Task Handle(RemovePostLike request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts.FindAsync(request.Id);

            if (post == null)
            {
                _logger.LogError("Post not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PostNotFound], HttpStatusCode.NotFound);
            }

            var userId = _userService.GetCurrentUserId();
            var postLike = await _dbContext.PostLikes.Where(e => e.PostId == request.Id && e.UserId == userId)
                                                     .FirstOrDefaultAsync(cancellationToken);

            if (postLike == null) return;

            post.LikesCount--;
            post.GlobalScore -= PostsFromGlobalPopularityRecommendationFeedService.LikesScore;
            _dbContext.PostLikes.Remove(postLike);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
