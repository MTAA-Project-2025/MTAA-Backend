using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Groups.Channels.CommandHandlers
{
    public class UpdateChannelImageHandler(MTAA_BackendDbContext dbContext,
        IImageService imageService,
        IMapper mapper) : IRequestHandler<UpdateChannelImage, MyImageGroupResponse>
    {
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IImageService _imageService = imageService;
        private readonly IMapper _mapper = mapper;

        public async Task<MyImageGroupResponse> Handle(UpdateChannelImage request, CancellationToken cancellationToken)
        {
            var channel = await _dbContext.Channels.Where(e => e.Id == request.Id)
                                                   .Include(e => e.Image)
                                                   .FirstOrDefaultAsync(cancellationToken);

            if (channel.Image != null)
            {
                var imageGroup = await _dbContext.ImageGroups.Where(e => e.Id == channel.ImageId)
                                                             .Include(e => e.Images)
                                                             .FirstOrDefaultAsync(cancellationToken);
                if (imageGroup != null)
                {
                    await _imageService.RemoveImageGroup(imageGroup, cancellationToken);
                    foreach (var image in imageGroup.Images)
                    {
                        _dbContext.Images.Remove(image);
                    }
                    _dbContext.ImageGroups.Remove(imageGroup);
                }
            }
            var newimageGroup = await _imageService.SaveImage(request.Image, ImageSavingTypes.ChannelImage, cancellationToken);
            foreach (var image in newimageGroup.Images)
            {
                _dbContext.Images.Add(image);
            }
            _dbContext.ImageGroups.Add(newimageGroup);
            channel.Image = newimageGroup;

            return _mapper.Map<MyImageGroupResponse>(newimageGroup);
        }
    }
}
