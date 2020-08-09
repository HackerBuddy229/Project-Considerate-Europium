using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Server.Wrappers;
using ProjectConsiderateEuropium.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;

namespace ProjectConsiderateEuropium.Server.services.ZeroProduct
{
    public interface IProductCreationService //TODO: Register
    {
        public CreationResult<Product> CreateProduct(Product product);
        public bool IsValidProduct(Product product);
    }
    public class ProductCreationService : IProductCreationService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductCreationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CreationResult<Product> CreateProduct(Product product)
        {
            if (!IsValidProduct(product))
                return new CreationResult<Product> { Errors = new List<string> { "Not a valid product" } };

            _dbContext.BeginTransaction();

            _dbContext.Products.Add(product);

            try
            {
                _dbContext.Commit();
                return new CreationResult<Product> { Created = product };

            } catch (Exception ex)
            {
                _dbContext.Rollback();
                return new CreationResult<Product> { Errors = new List<string> { $"Db error {ex.Message}"}};
            }
        }

        public bool IsValidProduct(Product product)
        {
            //not null
            if (product != null)
                if (!string.IsNullOrEmpty(product.Id))
                    if (!string.IsNullOrEmpty(product.Logo))
                        if (!string.IsNullOrEmpty(product.AccentColor))
                            if (!string.IsNullOrEmpty(product.Designation))
                                if (!string.IsNullOrEmpty(product.Contributor))
                                    return true;
            return false;
        }
    }
}
