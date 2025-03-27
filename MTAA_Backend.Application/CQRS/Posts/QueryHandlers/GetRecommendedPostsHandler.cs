using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Posts.RecommendationSystem;
using MTAA_Backend.Infrastructure;
using System.Collections.Generic;

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

            List<FullPostResponse> mappedPosts = new List<FullPostResponse>(totalCount);

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
                var newPosts = await _dbContext.RecommendationItems.Where(e => e.FeedId == feed.Item1.Id)
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
                                                        .Select(e => new
                                                        {
                                                            Post = e.Post,
                                                            IsLiked = e.Post.Likes.Any(e => e.UserId == user.Id)
                                                        })
                                                        .ToListAsync(cancellationToken);

                for (int i = 0; i < newPosts.Count; i++)
                {
                    var post = newPosts[i].Post;
                    var mapPost = _mapper.Map<FullPostResponse>(post);
                    mapPost.IsLiked = newPosts[i].IsLiked;

                    if (post.Owner.Avatar != null)
                    {
                        if (post.Owner.Avatar.CustomAvatar != null)
                        {
                            mapPost.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(post.Owner.Avatar.CustomAvatar);
                        }
                        else if (post.Owner.Avatar.PresetAvatar != null)
                        {
                            mapPost.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(post.Owner.Avatar.PresetAvatar);
                        }
                    }
                    mappedPosts.Add(mapPost);
                }
                posts.AddRange(newPosts.Select(e => e.Post).ToList());
            }
            if (totalCount > 0)
            {
                var newPosts = await _preferencesRecommendationService.GetRealTimeRecommendations(user.Id, totalCount);

                for (int i = 0; i < posts.Count; i++)
                {
                    var post = posts.ElementAt(i);
                    var mapPost = _mapper.Map<FullPostResponse>(post);

                    if (post.Owner.Avatar != null)
                    {
                        if (post.Owner.Avatar.CustomAvatar != null)
                        {
                            mapPost.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(post.Owner.Avatar.CustomAvatar);
                        }
                        else if (post.Owner.Avatar.PresetAvatar != null)
                        {
                            mapPost.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(post.Owner.Avatar.PresetAvatar);
                        }
                    }
                    mappedPosts.Add(mapPost);
                }
                posts.AddRange(newPosts);
            }

            await _mediator.Publish(new GetRecommendedPostsEvent()
            {
                Posts = posts,
                UserId = user.Id
            }, cancellationToken);

            return mappedPosts;
        }
    }
}
