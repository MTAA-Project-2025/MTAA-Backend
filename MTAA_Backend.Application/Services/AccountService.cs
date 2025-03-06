using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Infrastructure;
using System.Threading;

namespace MTAA_Backend.Application.Services
{
    public class AccountService(MTAA_BackendDbContext dbContext,
        IImageService imageService) : IAccountService
    {
        private readonly MTAA_BackendDbContext _dbContext = dbContext;
        private readonly IImageService _imageService = imageService;

        public async Task ChangePresetAvatar(UserPresetAvatarImage imageGroup, User user, CancellationToken cancellationToken = default)
        {
            if (user.Avatar == null)
            {
                var avatar = new UserAvatar()
                {
                    PresetAvatar = imageGroup
                };
                user.Avatar = avatar;
                _dbContext.UserAvatars.Add(avatar);
            }
            else
            {
                if (user.Avatar.CustomAvatar != null)
                {
                    var oldImageGroup = await _dbContext.ImageGroups.Where(e => e.Id == user.Avatar.CustomAvatarId)
                    .Include(e => e.Images)
                                                                 .FirstOrDefaultAsync(cancellationToken);
                    if (oldImageGroup != null)
                    {
                        await _imageService.RemoveImageGroup(oldImageGroup, cancellationToken);
                        foreach (var image in oldImageGroup.Images)
                        {
                            _dbContext.Images.Remove(image);
                        }
                        _dbContext.ImageGroups.Remove(oldImageGroup);
                    }
                }

                user.Avatar.PresetAvatar = imageGroup;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
