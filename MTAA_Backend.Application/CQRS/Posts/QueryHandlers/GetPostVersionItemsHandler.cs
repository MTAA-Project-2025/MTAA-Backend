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
using MTAA_Backend.Application.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MTAA_Backend.Application.CQRS.Posts.QueryHandlers
{
    public class GetPostVersionItemsHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService) : IRequestHandler<GetPostVersionItems, ICollection<VersionPostItemResponse>>
    {
        public async Task<ICollection<VersionPostItemResponse>> Handle(GetPostVersionItems request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var posts = await _dbContext.Posts.Where(e => e.OwnerId == userId)
                                              .OrderByDescending(e => e.DataCreationTime)
                                              .OrderByDescending(e => e.GlobalScore)
                                              .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                                              .Take(request.PageParameters.PageSize)
                                              .Select(e => new
                                              {
                                                  e.Id,
                                                  e.Version
                                              })
                                              .ToListAsync(cancellationToken);

            var mappedPosts = new List<VersionPostItemResponse>(posts.Count);
            foreach(var post in posts)
            {
                mappedPosts.Add(new VersionPostItemResponse()
                {
                    Id = post.Id,
                    Version = post.Version
                });
            }
            return mappedPosts;
        }
    }
}
