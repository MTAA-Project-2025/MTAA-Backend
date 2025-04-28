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
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using System.Collections.Generic;

namespace MTAA_Backend.Application.CQRS.Posts.QueryHandlers
{
    public class GetGlobalPostsHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper,
        IVectorDatabaseRepository _vectorDbRepository,
        IEmbeddingsService _embeddingsService) : IRequestHandler<GetGlobalPosts, ICollection<FullPostResponse>>
    {
        public async Task<ICollection<FullPostResponse>> Handle(GetGlobalPosts request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            Expression<Func<Post, bool>> filterCondition = e => !e.IsDeleted;

            int skipCount = 0;
            int takeCount = request.PageParameters.PageSize;
            Dictionary<Guid, float> filterPosts = new Dictionary<Guid, float>();
            if (request.FilterStr != null && request.FilterStr != "")
            {
                var vector = (await _embeddingsService.GetTextEmbeddings(request.FilterStr)).Select(x => (float)x).ToArray();
                var postPoints = (await _vectorDbRepository.GetPostVectors(VectorCollections.PostTextEmbeddings, vector, (ulong)request.PageParameters.PageSize, null, (ulong)(request.PageParameters.PageNumber * request.PageParameters.PageSize), cancellationToken: cancellationToken)).ToList();

                foreach (var postPoint in postPoints)
                {
                    filterPosts.Add(Guid.Parse(postPoint.Id.Uuid), postPoint.Score);
                }

                filterCondition = filterCondition.And(e => filterPosts.Keys.Any(id => id == e.Id));
                skipCount = 0;
            }
            else
            {
                skipCount = request.PageParameters.PageNumber * request.PageParameters.PageSize;
            }

            var posts = await _dbContext.Posts.Where(filterCondition)
                                              .OrderByDescending(e => e.GlobalScore)
                                              .Skip(skipCount)
                                              .Take(takeCount)
                                              .Include(e => e.Location)
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
                                              .Select(e => new
                                              {
                                                  Post = e,
                                                  IsLiked = e.Likes.Any(e => e.UserId == userId)
                                              })
                                              .ToListAsync(cancellationToken);

            var mappedPosts = new List<FullPostResponse>(posts.Count);
            for (int i = 0; i < posts.Count; i++)
            {
                var post = posts[i].Post;
                var mappedPost = _mapper.Map<FullPostResponse>(post);
                mappedPost.IsLiked = posts[i].IsLiked;

                if (post.Location != null)
                {
                    mappedPost.LocationId = post.Location.Id;
                }
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
                mappedPosts.Add(mappedPost);
            }

            if (filterPosts.Count != 0)
            {
                mappedPosts = mappedPosts.OrderBy(e => filterPosts[e.Id]).ToList();
            }

            return mappedPosts;
        }
    }
}
