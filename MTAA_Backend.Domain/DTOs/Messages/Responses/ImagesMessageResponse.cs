using MTAA_Backend.Domain.DTOs.Images.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Messages.Responses
{
    public class ImagesMessageResponse : TextMessageResponse
    {
        public ICollection<MyImageGroupResponse> Images { get; set; } = new List<MyImageGroupResponse>();
    }
}
