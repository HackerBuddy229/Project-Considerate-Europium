using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectConsiderateEuropium.Shared.Filter;
using ProjectConsiderateEuropium.Shared.Models;
using ProjectConsiderateEuropium.Shared.Models.ModelTypes;

namespace ProjectConsiderateEuropium.Server.services.AlternativeProduct
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlternativeController : ControllerBase
    {
        private readonly IAlternativeService _alternativeService;

        public AlternativeController(IAlternativeService alternativeService)
        {
            _alternativeService = alternativeService;
        }


        [HttpGet("by-id/{id}")]
        public IActionResult GetAlternativeById(string id)
        {
            var result = _alternativeService.GetAlternative(id, IdentificationType.Id);
            if (result == null)
                return BadRequest("no such alternative");

            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateAlternative([FromBody] Alternative alternative)
        {
            return Ok("not working");
        }

        //[HttpGet("paged/newest")]
        //public IActionResult GetPaginatedAlternatives([FromQuery] PaginationFilter filter)
        //{
        //    if (filter == null)
        //        filter = new PaginationFilter(1, 2);
        //    var route = Request.Path.Value;
        //    return Ok(_alternativeService.GetPagedAlternativesByNewest(filter, route));
        //}

        [HttpGet("paged/newest")]
        public IActionResult GetPaginatedAlternatives([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var filter = new PaginationFilter(pageNumber, pageSize);

            var route = Request.Path.Value;
            return Ok(_alternativeService.GetPagedAlternativesByNewest(filter, route));
        }
    }
}
