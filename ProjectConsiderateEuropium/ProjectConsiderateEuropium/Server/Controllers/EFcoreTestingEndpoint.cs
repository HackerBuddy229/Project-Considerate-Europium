using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Shared.Models.Testing;

namespace ProjectConsiderateEuropium.Server.Controllers
{
    [Route("api/EFCoreTest")]
    [ApiController]
    public class EFCoreTestingEndpoint : ControllerBase
    {
        private readonly EFCoreTestingService _efCoreTestingService;

        public EFCoreTestingEndpoint(EFCoreTestingService efCoreTestingService)
        {
            _efCoreTestingService = efCoreTestingService;
        }

        [HttpGet]
        public IActionResult GetTestItem()
        {
            return Ok(_efCoreTestingService.GeTestItem());
        }
    }

    public class EFCoreTestingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _itemId;

        public EFCoreTestingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            var item = new DbTestItem(){State = true};
            _itemId = item.Id;

            _dbContext.TestItems.Add(item);
            _dbContext.SaveChanges();
        }

        public DbTestItem GeTestItem()
        {
            return _dbContext.TestItems.FirstOrDefault(i => i.Id == _itemId);
        }


    }
}
