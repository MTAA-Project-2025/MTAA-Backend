using MTAA_Backend.Domain.Resources.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces
{
    public interface IVersionItemService
    {
        Task InitializationForUserAsync(string userId, CancellationToken cancellationToken = default);
    }
}
