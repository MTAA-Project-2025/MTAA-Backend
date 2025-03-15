using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Posts.Queries;
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
        IMapper _mapper) : IRequestHandler<GetAccountPosts, ICollection<SimplePostResponse>>
    {
        public async Task<ICollection<SimplePostResponse>> Handle(GetAccountPosts request, CancellationToken cancellationToken)
        {
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
                                              })
                                              .ToListAsync(cancellationToken);

            var mappedPosts = new List<SimplePostResponse>(posts.Count);

            foreach(var post in posts)
            {
                mappedPosts.Add(new SimplePostResponse()
                {
                    Id = post.id,
                    SmallFirstImage = _mapper.Map<MyImageResponse>(post.image)
                });
            }

            return mappedPosts;
        }
    }
}
