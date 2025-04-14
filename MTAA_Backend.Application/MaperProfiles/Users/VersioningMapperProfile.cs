using MTAA_Backend.Domain.DTOs.Versioning.Responses;
using MTAA_Backend.Domain.Entities.Versions;
using MTAA_Backend.Domain.Resources.Versioning;

namespace MTAA_Backend.Application.MaperProfiles.Users
{
    public class VersioningMapperProfile:AutoMapper.Profile
    {
        public VersioningMapperProfile()
        {
            CreateMap<VersionItem, VersionItemResponse>();
        }
    }
}
