using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Entities.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.MaperProfiles.Images
{
    public class ImagesMapperProfile : AutoMapper.Profile
    {
        public ImagesMapperProfile()
        {
            CreateMap<MyImageGroup, MyImageGroupResponse>();
            CreateMap<MyImage, MyImageResponse>();
        }
    }
}
