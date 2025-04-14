using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.DTOs.Shared.Requests
{
    public class PageParameters
    {
        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        public int Offset { get; set; } = 0;

        private int pageSize = 10;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value <= 0) pageSize = 1;
                else if (value > maxPageSize) pageSize = maxPageSize;
                else pageSize = value;
            }
        }
    }
}
