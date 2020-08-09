using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Server.Wrappers;
using ProjectConsiderateEuropium.Shared.Filter;
using ProjectConsiderateEuropium.Shared.Models;
using ProjectConsiderateEuropium.Shared.Models.ModelTypes;
using ProjectConsiderateEuropium.Shared.Wrappers;

namespace ProjectConsiderateEuropium.Server.services.ZeroProduct
{
    public interface IProductService
    {
        Product GetProduct(string identifier, IdentificationType identificationType);
        PagedResponse<IEnumerable<Product>> GetPagedProductsByNewest(PaginationFilter filter, string route);
        CreationResult<Product> CreateProduct(Product product);
    }

    public class ProductService : IProductService
    {
        private readonly IProductGetterService _productGetterService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductCreationService _productCreationService;

        public ProductService(IProductGetterService productGetterService, ApplicationDbContext dbContext,
            IProductCreationService productCreationService)
        {
            _productGetterService = productGetterService;
            _dbContext = dbContext;
            _productCreationService = productCreationService;
        }


        public Product GetProduct(string identifier, IdentificationType identificationType)
        {
            return identificationType switch
            {
                IdentificationType.OrganizationIdentifier =>
                    _productGetterService.GetProductByDesignation(identifier),

                IdentificationType.Designation =>
                    _productGetterService.GetProductByDesignation(identifier),

                IdentificationType.Id =>
                    _productGetterService.GetProductById(identifier),

                _ => null
            };
        }

        public PagedResponse<IEnumerable<Product>> GetPagedProductsByNewest(PaginationFilter filter, string route)
        {
            return _productGetterService.GetPagedProductByDescendingCreated(filter, route);
        }

        public CreationResult<Product> CreateProduct(Product product)
        {
            return _productCreationService.CreateProduct(product);
        }
    }
}
