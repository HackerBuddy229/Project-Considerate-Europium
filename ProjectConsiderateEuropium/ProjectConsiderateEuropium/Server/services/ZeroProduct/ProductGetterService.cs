using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Server.Helpers;
using ProjectConsiderateEuropium.Shared.Filter;
using ProjectConsiderateEuropium.Shared.Models;
using ProjectConsiderateEuropium.Shared.Wrappers;

namespace ProjectConsiderateEuropium.Server.services.ZeroProduct
{
    public interface IProductGetterService
    {
        Product GetProductById(string id);
        Product GetProductByDesignation(string designation);
        Product GetProductByOrganizationalIdentifier(string organizationalIdentifier);

        PagedResponse<IEnumerable<Product>> GetPagedProductByDescendingCreated(PaginationFilter filter, string route);
    }

    public class ProductGetterService : IProductGetterService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUriService _uriService;

        public ProductGetterService(ApplicationDbContext dbContext, IUriService uriService)
        {
            _dbContext = dbContext;
            _uriService = uriService;
        }


        public Product GetProductById(string id)
        {
            return _dbContext.Products.FirstOrDefault(x => x.Id == id);
        }

        public Product GetProductByDesignation(string designation)
        {
            return _dbContext.Products.FirstOrDefault(x => x.Designation == designation);
        }

        public Product GetProductByOrganizationalIdentifier(string organizationalIdentifier)
        {
            return _dbContext.Products.FirstOrDefault(x => x.OrganizationIdentifier == organizationalIdentifier);
        }



        public PagedResponse<IEnumerable<Product>> GetPagedProductByDescendingCreated(PaginationFilter filter, string route)
        {
            var validFilter = new PaginationFilter(filter);
            var pagedDate = _dbContext.Products
                .OrderByDescending(x => DateTime.Parse(x.CreatedAt).Ticks)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = _dbContext.Products.Count();

            var pagedResponse = PaginationHelper.CreatePagedResponse<Product>(
                pagedDate, validFilter, totalRecords, _uriService, route);
            return pagedResponse;

        }
    }
}
