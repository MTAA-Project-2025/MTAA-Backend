using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Entities.Versions;
using MTAA_Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Infrastructure.Seeders.Versions
{
    public class VersionItemSeeder(MTAA_BackendDbContext _dbContext) : IVersionItemSeeder
    {
        public async Task SeedForUserAsync(string userId, CancellationToken cancellationToken)
        {
            var existingItems = await _dbContext.VersionItems.Where(v => v.UserId == userId).Select(v => v.Id).ToListAsync(cancellationToken);

            foreach (VersionItemType type in Enum.GetValues(typeof(VersionItemType)))
            {
                if (!existingItems.Contains(type))
                {
                    _dbContext.VersionItems.Add(new VersionItem { Id = type, Version = 1, UserId = userId });
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
