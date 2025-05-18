using AutoMapper;
using MediatR;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using MTAA_Backend.Infrastructure;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MTAA_Backend.Application.CQRS.Posts.QueryHandlers
{
    public class GetLikedPostsHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper) : IRequestHandler<GetLikedPosts, ICollection<FullPostResponse>>
    {
        public async Task<ICollection<FullPostResponse>> Handle(GetLikedPosts request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var posts = await _dbContext.PostLikes.Where(e=>e.UserId==userId)
                                                  .OrderByDescending(e => e.DataCreationTime)
                                                  .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                                                  .Take(request.PageParameters.PageSize)
                                                  .Include(e => e.Post)
                                                    .ThenInclude(e=>e.Location)
                                                  .Include(e=>e.Post)
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
                                                  .ToListAsync(cancellationToken);

            var mappedPosts = new List<FullPostResponse>(posts.Count);
            for (int i = 0; i < posts.Count; i++)
            {
                var post = posts[i];
                if (post.IsHidden && post.OwnerId != userId) continue;
                var mappedPost = _mapper.Map<FullPostResponse>(post);
                mappedPost.IsLiked = true;

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

            return mappedPosts;
        }
    }
}
