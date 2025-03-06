using Microsoft.Extensions.Localization;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces.RecomendationSystem.RecomendationFeedService;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Services.RecomendationSystem.RecomendationFeedServices
{
    public class PostsFromFollowersRecomendationFeedService : IPostsFromFollowersRecomendationFeedService
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public PostsFromFollowersRecomendationFeedService(MTAA_BackendDbContext dbContext,
            ILogger logger,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _dbContext = dbContext;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task RecomendPost(Guid postId, string userId)
        {
            var post = await _dbContext.Posts.FindAsync(postId);
            if (post == null)
            {
                _logger.LogError($"Post {postId} not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.PostNotFound], HttpStatusCode.NotFound);
            }

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogError($"User {userId} not found");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.NotFound);
            }
        }

        public async Task RecomendPostBackgroundJob(Post post, string userId)
        {

        }
    }
}
