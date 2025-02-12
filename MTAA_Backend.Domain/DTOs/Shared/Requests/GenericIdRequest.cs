using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Shared.Requests
{
    public class GenericIdRequest<T> where T : struct
    {
        public T Id { get; set; }
    }
}
