using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Posts.RecommendationSystem;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Posts.QueryHandlers
{
    public class GetRecommendedPostsHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper,
        IMediator _mediator,
        IPostsFromPreferencesRecommendationFeedService _preferencesRecommendationService) : IRequestHandler<GetRecommendedPosts, ICollection<FullPostResponse>>
    {
        public async Task<ICollection<FullPostResponse>> Handle(GetRecommendedPosts request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentUser();
            var localFeeds = await _dbContext.LocalRecommendationFeeds.Where(e => e.UserId == user.Id)
                                                                     .ToListAsync(cancellationToken);

            var globalRecommendationFeed = await _dbContext.SharedRecommendationFeeds.Where(e => e.Type == RecommendationFeedTypes.PostsFromGlobalPopularityFeed)
                                                                                   .FirstOrDefaultAsync(cancellationToken);

            var requestFeeds = new List<(BaseRecommendationFeed, int, int)>(); //first int - count that feed have, second int - count that feed requested
            int totalCount = request.PageParameters.PageSize;

            List<Post> posts = new List<Post>(totalCount);

            foreach (var feed in localFeeds)
            {
                int requestedSize = (int)(totalCount * feed.Weight);
                requestFeeds.Add((feed, feed.RecommendationItemsCount, requestedSize));
            }
            int requestedGlobalSize = (int)(totalCount * globalRecommendationFeed.Weight);
            requestFeeds.Add((globalRecommendationFeed, globalRecommendationFeed.RecommendationItemsCount - user.GlobalRecommendationsFeedIndex, requestedGlobalSize));

            int existedPostsCount = requestFeeds.Sum(e => e.Item2);

            for (int i = 0; i < requestFeeds.Count; i++)
            {
                if (requestFeeds[i].Item2 < requestFeeds[i].Item3)
                {
                    requestFeeds[i] = (requestFeeds[i].Item1, requestFeeds[i].Item2, requestFeeds[i].Item2);
                }
                totalCount -= requestFeeds[i].Item3;
            }
            if (totalCount > 0)
            {
                for (int i = 0; i < requestFeeds.Count; i++)
                {
                    if (totalCount <= 0) break;

                    int extra = 0;
                    if (requestFeeds[i].Item2 > requestFeeds[i].Item3)
                    {
                        extra = Math.Min(requestFeeds[i].Item2 - requestFeeds[i].Item3, totalCount);
                    }
                    requestFeeds[i] = (requestFeeds[i].Item1, requestFeeds[i].Item2, requestFeeds[i].Item3 + extra);
                    totalCount -= extra;
                }
            }
            foreach (var feed in requestFeeds)
            {
                posts.AddRange(await _dbContext.RecommendationItems.Where(e => e.FeedId == feed.Item1.Id)
                                                        .OrderByDescending(e => e.LocalScore)
                                                        .Take(feed.Item3)
                                                        .Include(e => e.Post)
                                                            .ThenInclude(e => e.Owner)
                                                                .ThenInclude(e => e.Avatar)
                                                                    .ThenInclude(e => e.CustomAvatar)
                                                                        .ThenInclude(e => e.Images)
                                                        .Include(e => e.Post)
                                                            .ThenInclude(e => e.Owner)
                                                                .ThenInclude(e => e.Avatar)
                                                                    .ThenInclude(e => e.PresetAvatar)
                                                                        .ThenInclude(e => e.Images)
                                                        .Include(e => e.Post)
                                                            .ThenInclude(e => e.Images)
                                                                .ThenInclude(e => e.Images)
                                                        .Select(e => e.Post)
                                                        .ToListAsync(cancellationToken));
            }
            if (totalCount > 0)
            {
                posts.AddRange(await _preferencesRecommendationService.GetRealTimeRecommendations(user.Id, totalCount));
            }

            await _mediator.Publish(new GetRecommendedPostsEvent()
            {
                Posts = posts,
                UserId = user.Id
            }, cancellationToken);

            var mappedPosts = _mapper.Map<List<FullPostResponse>>(posts);
            for (int i = 0; i < mappedPosts.Count; i++)
            {
                var mappedPost = mappedPosts[i];
                var post = posts[i];

                if (post.Owner.Avatar != null)
                {
                    if (post.Owner.Avatar.CustomAvatar != null)
                    {
                        mappedPost.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(post.Owner.Avatar.CustomAvatar);
                    }
                    else if (post.Owner.Avatar.PresetAvatar != null)
                    {
                        mappedPost.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(post.Owner.Avatar.PresetAvatar);
                    }
                }
            }

            return mappedPosts;
        }
    }
}
