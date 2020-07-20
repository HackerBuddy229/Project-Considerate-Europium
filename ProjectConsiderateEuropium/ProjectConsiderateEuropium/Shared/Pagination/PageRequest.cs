using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Pagination
{
    public class PageRequest<T>
    {
        public PageRequest(PageRequest<T> unsafeRequest) // creates a safe request from an unsafe request
        {
            AmountOfItems = unsafeRequest.AmountOfItems <= 25 ? unsafeRequest.AmountOfItems : 25;
            PageNumber = unsafeRequest.PageNumber >= 1 ? unsafeRequest.PageNumber : 1;
            Request = unsafeRequest.Request;
        }

        public PageRequest() //empty
        {
        }


        public T Request { get; set; }

        //page specification
        public int PageNumber { get; set; }
        public int AmountOfItems { get; set; }

    }
}
