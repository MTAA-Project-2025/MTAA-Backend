using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Images.PresetImages.Queries;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Users.Identity.Other;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Images.PresetImages.QueryHandlers
{
    public class GetAllPresetImagesHandler(IDistributedCache _distributedCache,
        IMapper _mapper,
        MTAA_BackendDbContext _dbContext) : IRequestHandler<GetAllPresetImages, ICollection<MyImageGroupResponse>>
    {
        public async Task<ICollection<MyImageGroupResponse>> Handle(GetAllPresetImages request, CancellationToken cancellationToken)
        {
            var recordId = "PresetAvatarImages";
            var response = await _distributedCache.GetRecordAsync<ICollection<MyImageGroupResponse>>(recordId);

            if (response != null) return response;

            var images = await _dbContext.UserPresetAvatarImages.Include(e => e.Images).ToListAsync(cancellationToken);

            response = _mapper.Map<ICollection<MyImageGroupResponse>>(images);
            await _distributedCache.SetRecordAsync(recordId, response);

            return response;
        }
    }
}
