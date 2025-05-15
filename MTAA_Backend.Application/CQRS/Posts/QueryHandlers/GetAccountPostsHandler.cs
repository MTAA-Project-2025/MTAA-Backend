using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Entities.Posts;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Linq.Expressions;

namespace MTAA_Backend.Application.CQRS.Posts.QueryHandlers
{
    public class GetAccountPostsHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper,
        IDistributedCache _distributedCache) : IRequestHandler<GetAccountPosts, ICollection<SimplePostResponse>>
    {
        public async Task<ICollection<SimplePostResponse>> Handle(GetAccountPosts request, CancellationToken cancellationToken)
        {
            if (request.PageParameters.PageNumber == 0)
            {
                var recordId = "Account_Posts_" + request.UserId;
                var redisPosts = await _distributedCache.GetRecordAsync<ICollection<SimplePostResponse>>(recordId);
                if (redisPosts != null && redisPosts.Count > 0) return redisPosts;
            }

            var userId = _userService.GetCurrentUserId();

            var posts = await _dbContext.Posts.Where(e => e.OwnerId == request.UserId)
                                              .OrderByDescending(e => e.DataCreationTime)
                                              .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                                              .Take(request.PageParameters.PageSize)
                                              .Include(e => e.Images)
                                                  .ThenInclude(e => e.Images)
                                              .Select(e => new
                                              {
                                                  id = e.Id,
                                                  image = e.Images.First().Images.Where(e => e.Type == ImageSizeType.Small).FirstOrDefault(),
                                                  DataCreationTime = e.DataCreationTime,
                                                  OwnerId = e.OwnerId,
                                                  IsHidden = e.IsHidden
                                              })
                                              .ToListAsync(cancellationToken);

            var mappedPosts = new List<SimplePostResponse>(posts.Count);

            foreach(var post in posts)
            {
                if (userId != post.OwnerId && post.IsHidden) continue;
                mappedPosts.Add(new SimplePostResponse()
                {
                    Id = post.id,
                    SmallFirstImage = _mapper.Map<MyImageResponse>(post.image),
                    DataCreationTime = post.DataCreationTime
                });
            }

            if (request.PageParameters.PageNumber == 0 && posts.Count > 0 && userId != posts.First().OwnerId)
            {
                var recordId = "Account_Posts_" + request.UserId;
                await _distributedCache.SetRecordAsync(recordId, mappedPosts, absoluteExpireTime: TimeSpan.FromMinutes(1));
            }

            return mappedPosts;
        }
    }
}
