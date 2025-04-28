using MTAA_Backend.Application.CQRS.Locations.Commands;
using MTAA_Backend.Application.CQRS.Locations.Queries;
using MTAA_Backend.Application.MaperProfiles.Posts;
using MTAA_Backend.Domain.DTOs.Locations.Requests;
using MTAA_Backend.Domain.DTOs.Posts.Requests;

namespace MTAA_Backend.Application.MaperProfiles.Locations
{
    public class LocationsMapperProfile : AutoMapper.Profile
    {
        public LocationsMapperProfile()
        {
            CreateMap<GetLocationPointsRequest, GetLocationPoints>();
            CreateMap<AddLocationRequest, UpdatePostLocation>();
        }
    }
}
