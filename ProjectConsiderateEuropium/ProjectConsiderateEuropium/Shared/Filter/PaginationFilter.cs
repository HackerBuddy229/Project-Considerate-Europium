using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Filter
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }


        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10  || pageSize < 1 ? 10 : pageSize;
        }
        public PaginationFilter(PaginationFilter filter)
        {
            this.PageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
            this.PageSize = filter.PageSize > 10 || filter.PageSize < 1 ? 10 : filter.PageSize;
        }
    }
}
