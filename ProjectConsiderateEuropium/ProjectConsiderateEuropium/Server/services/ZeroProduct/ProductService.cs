using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProjectConsiderateEuropium.Client;
using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Server.handlers;
using ProjectConsiderateEuropium.Server.Wrappers;
using ProjectConsiderateEuropium.Shared.Communication;
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
        CreationResult<Product> CreateProduct(UserEntityCreationRequest<Product> request);
    }

    public class ProductService : IProductService
    {
        private readonly IProductGetterService _productGetterService;
        private readonly IProductCreationService _productCreationService;
        private readonly IUserImageHandler _userImageHandler;
        private readonly ApplicationDbContext _dbContext;

        public ProductService(IProductGetterService productGetterService,
            IProductCreationService productCreationService,
            IUserImageHandler userImageHandler,
            ApplicationDbContext dbContext)
        {
            _productGetterService = productGetterService;
            _productCreationService = productCreationService;
            _userImageHandler = userImageHandler;
            _dbContext = dbContext;
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

        public CreationResult<Product> CreateProduct(UserEntityCreationRequest<Product> product)
        {

            //verify file
            if (!_userImageHandler.ValidateImage(product.Image))
                return new CreationResult<Product> { Errors = new List<string> { "Invalid image" } };

            //get path
            product.Image = _userImageHandler.CreateUserImagePath(product.Image);
            
            product.NewEntity.Logo = product.Image.Path;

            //createProduct
            var result = _productCreationService.CreateProduct(product.NewEntity);


            //verify Entry
            if (!result.Succeeded)
                return new CreationResult<Product>{Errors = new List<string>{"bad product"}};


            //write image
            try
            {
                _userImageHandler.WriteImage(product.Image);
            }
            catch (IOException ex)
            {
                _dbContext.Products.Remove(result.Created);
                _dbContext.SaveChanges();
                return new CreationResult<Product>{Errors = new List<string>{$"error writing img {ex.Message}"}};
            }

            //return Result
            return new CreationResult<Product>{Created = result.Created};
        }
    }
}
