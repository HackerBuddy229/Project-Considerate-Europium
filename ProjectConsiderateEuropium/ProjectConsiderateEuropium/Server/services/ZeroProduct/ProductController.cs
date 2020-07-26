using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectConsiderateEuropium.Shared.Filter;
using ProjectConsiderateEuropium.Shared.Models;
using ProjectConsiderateEuropium.Shared.Models.ModelTypes;

namespace ProjectConsiderateEuropium.Server.services.ZeroProduct
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        
        [HttpGet("by-id/{id}")]
        public IActionResult GetProductById(string id)
        {
            return Ok(_productService.GetProduct(id, IdentificationType.Id));
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            var result = _productService.CreateProduct(product);
            if (!result.Succeeded)
                return BadRequest();

            return Ok(result.Created);
        }

        [HttpGet("paged/newest")]
        public IActionResult GetPagedProductByNewest([FromQuery] int pageNumber, int pageSize)
        {
            var route = Request.Path.Value;
            var filter = new PaginationFilter(pageNumber, pageSize);

            var result = _productService.GetPagedProductsByNewest(filter, route);
            return Ok(result);
        }
    }
}
