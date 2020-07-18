using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Pagination
{
    public class PagedResponse<T>
    {

        public IEnumerable<T> Content { get; set; }

        //current page information
        public int CurrentPage { get; set; }
        public int AmountOfItems { get; set; }

        //page url:s
        public string FirstPage { get; set; }
        public string PreviousPage { get; set; }
        public string NextPage { get; set; }
        public string LastPage { get; set; }

    }
}
