using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces
{
    public interface IAccountService
    {
        public Task ChangePresetAvatar(UserPresetAvatarImage imageGroup, User user, CancellationToken cancellationToken = default);
    }
}
