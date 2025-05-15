using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Locations.Responses;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Other;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Posts.QueryHandlers
{
    public class GetSchedulePostsHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper) : IRequestHandler<GetSchedulePosts, ICollection<SchedulePostResponse>>
    {
        public async Task<ICollection<SchedulePostResponse>> Handle(GetSchedulePosts request, CancellationToken cancellationToken)
        {
            string userId = _userService.GetCurrentUserId();

            var posts = await _dbContext.Posts.Where(e => e.OwnerId == userId && e.IsHidden)
                                              .OrderByDescending(e => e.DataCreationTime)
                                              .Skip(request.PageParameters.PageNumber * request.PageParameters.PageSize)
                                              .Take(request.PageParameters.PageSize)
                                              .Include(e => e.Images)
                                                  .ThenInclude(e => e.Images)
                                              .Select(p => new
                                              {
                                                  Id = p.Id,
                                                  DataCreationTime = p.DataCreationTime,
                                                  Description = p.Description,
                                                  Image = p.Images.First(),
                                                  Version = p.Version,
                                                  IsHidden = p.IsHidden,
                                                  HiddenReason = p.HiddenReason,
                                                  SchedulePublishDate = p.SchedulePublishDate
                                              })
                                              .ToListAsync(cancellationToken);


            var mappedPosts = new List<SchedulePostResponse>(posts.Count);

            foreach (var post in posts)
            {
                MyImageResponse img = null;
                var origImg = post.Image.Images.Where(e => e.Type == ImageSizeType.Small).First();
                if (origImg != null)
                {
                    img = _mapper.Map<MyImageResponse>(origImg);
                }
                

                mappedPosts.Add(new SchedulePostResponse()
                {
                    Id = post.Id,
                    SmallFirstImage = img,
                    DataCreationTime = post.DataCreationTime,
                    Description = post.Description,
                    HiddenReason = post.HiddenReason,
                    IsHidden = post.IsHidden,
                    SchedulePublishDate = post.SchedulePublishDate,
                    Version=post.Version,
                });
            }

            return mappedPosts;
        }
    }
}
