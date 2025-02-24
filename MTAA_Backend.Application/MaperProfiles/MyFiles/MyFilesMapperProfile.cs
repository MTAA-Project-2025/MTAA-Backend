using MTAA_Backend.Domain.DTOs.Files.Responses;
using MTAA_Backend.Domain.Entities.Files;

namespace MTAA_Backend.Application.MaperProfiles.MyFiles
{
    public class MyFilesMapperProfile : AutoMapper.Profile
    {
        public MyFilesMapperProfile()
        {
            CreateMap<MyFile, MyFileResponse>();
        }
    }
}
