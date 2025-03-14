using AutoMapper;
using MediatR;
using MTAA_Backend.Application.CQRS.Posts.Events;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Entities.Posts.RecommendationSystem;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Posts.RecommendationSystem;
using MTAA_Backend.Infrastructure;
using Azure;
using System.Linq.Expressions;
using MTAA_Backend.Application.Extensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.DTOs.Images.Response;

namespace MTAA_Backend.Application.CQRS.Posts.QueryHandlers
{
    public class GetGlobalPostsHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper) : IRequestHandler<GetGlobalPosts, ICollection<FullPostResponse>>
    {
        public async Task<ICollection<FullPostResponse>> Handle(GetGlobalPosts request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            Expression<Func<Post, bool>> filterCondition = e => !e.IsDeleted;

            if (request.FilterStr != null && request.FilterStr != "")
            {
                filterCondition = filterCondition.And(e => e.Description.Contains(request.FilterStr));
            }

            var likedPostIds = await _dbContext.PostLikes.Where(e => e.UserId == userId)
                                                         .Select(e => e.Id)
                                                         .ToListAsync(cancellationToken);

            var posts = await _dbContext.Posts.Where(filterCondition)
                                              .OrderByDescending(e => e.GlobalScore)
                                              .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                                              .Take(request.PageParameters.PageSize)
                                              .Include(e => e.Owner)
                                                  .ThenInclude(e => e.Avatar)
                                                      .ThenInclude(e => e.CustomAvatar)
                                                          .ThenInclude(e => e.Images)
                                              .Include(e => e.Owner)
                                                  .ThenInclude(e => e.Avatar)
                                                      .ThenInclude(e => e.PresetAvatar)
                                                          .ThenInclude(e => e.Images)
                                              .Include(e => e.Images)
                                                  .ThenInclude(e => e.Images)
                                              .ToListAsync(cancellationToken);

            var mappedPosts = _mapper.Map<List<FullPostResponse>>(posts);
            for(int i = 0; i < mappedPosts.Count; i++)
            {
                var mappedPost = mappedPosts[i];
                var post = posts[i];
                mappedPost.IsLiked = likedPostIds.Contains(post.Id);

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
