using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Images.Response
{
    public class MyImageGroupResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }

        public ICollection<MyImageResponse> Images { get; set; } = new List<MyImageResponse>();
    }
}
