using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Locations.Responses;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Posts.QueryHandlers
{
    public class GetSchedulePostByIdHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper) : IRequestHandler<GetSchedulePostById, SchedulePostResponse?>
    {
        public async Task<SchedulePostResponse?> Handle(GetSchedulePostById request, CancellationToken cancellationToken)
        {
            string userId = _userService.GetCurrentUserId();

            var post = await _dbContext.Posts.Where(e => e.OwnerId == userId && e.IsHidden && e.Id==request.Id)
                                              .OrderByDescending(e => e.DataCreationTime)
                                              .Include(e => e.Images)
                                                  .ThenInclude(e => e.Images)
                                              .Select(p => new
                                              {
                                                  Id = p.Id,
                                                  DataCreationTime = p.DataCreationTime,
                                                  Description = p.Description,
                                                  Image = p.Images.First(),
                                                  OwnerId = p.OwnerId,
                                                  Version = p.Version,
                                                  IsHidden = p.IsHidden,
                                                  HiddenReason = p.HiddenReason,
                                                  SchedulePublishDate = p.SchedulePublishDate
                                              })
                                              .FirstOrDefaultAsync(cancellationToken);


            if (post == null || userId!=post.OwnerId) return null;

            MyImageResponse img = null;
            var origImg = post.Image.Images.Where(e => e.Type == ImageSizeType.Small).First();
            if (origImg != null)
            {
                img = _mapper.Map<MyImageResponse>(origImg);
            }


            return new SchedulePostResponse()
            {
                Id = post.Id,
                SmallFirstImage = img,
                DataCreationTime = post.DataCreationTime,
                Description = post.Description,
                HiddenReason = post.HiddenReason,
                IsHidden = post.IsHidden,
                SchedulePublishDate = post.SchedulePublishDate,
                Version = post.Version,
            };
        }
    }
}
