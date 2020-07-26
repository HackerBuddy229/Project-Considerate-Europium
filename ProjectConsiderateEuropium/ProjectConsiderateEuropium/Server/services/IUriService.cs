using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectConsiderateEuropium.Shared.Filter;

namespace ProjectConsiderateEuropium.Server.services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
