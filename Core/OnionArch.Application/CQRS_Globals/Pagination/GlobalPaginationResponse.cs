using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.CQRS_Globals.Pagination
{
    public  class GlobalPaginationResponse: GlobalResponseResult
    {
        public int TotalCount { get; set; }
        public int TotalPageSize { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }
        public int PageSize { get; set; }
    }
}
