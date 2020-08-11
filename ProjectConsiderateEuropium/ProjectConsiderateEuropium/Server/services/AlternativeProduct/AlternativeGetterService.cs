using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ProjectConsiderateEuropium.Client;
using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Server.Helpers;
using ProjectConsiderateEuropium.Shared.Filter;
using ProjectConsiderateEuropium.Shared.Models;
using ProjectConsiderateEuropium.Shared.Wrappers;

namespace ProjectConsiderateEuropium.Server.services.AlternativeProduct
{
    public interface IAlternativeGetterService
    {
        Alternative GetAlternativeById(string id);
        Alternative GetAlternativeByDesignation(string designation);
        Alternative GetAlternativeByOrganizationIdentifier(string organizationIdentifier);
        IEnumerable<Alternative> GetAlternativesByDescendingCreated(int amount);
        IEnumerable<Alternative> GetAlternativesByAscendingCreated(int amount);
        PagedResponse<IEnumerable<Alternative>> GetPagedAlternativesByDescendingCreated(PaginationFilter filter, string route);

        Product PopulateProduct(Product product);

    }

    public class AlternativeGetterService : IAlternativeGetterService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUriService _uriService;

        public AlternativeGetterService(ApplicationDbContext dbContext, IUriService uriService)
        {
            _dbContext = dbContext;
            _uriService = uriService;
        }

        public Alternative GetAlternativeByDesignation(string designation)
        {
            return 
                _dbContext.Alternatives
                    .FirstOrDefault(a => a.Designation == designation);
        }

        public Alternative GetAlternativeByOrganizationIdentifier(string organizationIdentifier)
        {
            return
                _dbContext.Alternatives
                    .FirstOrDefault(a => a.OrganizationIdentifier == organizationIdentifier);
        }

        public IEnumerable<Alternative> GetAlternativesByDescendingCreated(int amount = 25)//newest
        {
            amount = amount < 101 ? amount : 100; //secure amount

            return _dbContext.Alternatives
                .OrderByDescending(a => DateTime.Parse(a.CreatedAt).Second)
                .Take(amount)
                .ToList();
        }

        public IEnumerable<Alternative> GetAlternativesByAscendingCreated(int amount)//oldest
        {
            amount = amount < 101 ? amount : 100; //secure amount

            return _dbContext.Alternatives
                .OrderBy(a => DateTime.Parse(a.CreatedAt).Second)
                .Take(amount)
                .ToList();
        }

        public PagedResponse<IEnumerable<Alternative>> GetPagedAlternativesByDescendingCreated(PaginationFilter filter, string route)
        {
            var validFilter = new PaginationFilter(filter);
            var pagedData = _dbContext.Alternatives
                .OrderByDescending(x=> DateTime.Parse(x.CreatedAt, CultureInfo.InvariantCulture).Ticks)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = _dbContext.Alternatives.Count();

            var pagedResponse = PaginationHelper.CreatePagedResponse<Alternative>(
                pagedData, validFilter, totalRecords, _uriService, route);
            return pagedResponse;
        }

        public Product PopulateProduct(Product product)
        {
            var alternativeIds = product.AlternativeIds.Split(",");
            if (string.IsNullOrWhiteSpace(product.AlternativeIds) || !alternativeIds.Any())
                return product;

            var alternatives = 
                alternativeIds.Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(alternative => _dbContext.Alternatives.FirstOrDefault(a => a.Id == alternative))
                    .Where(alt => alt != null)
                    .ToList();

            product.Alternatives = alternatives;
            return product;
        }

        public Alternative GetAlternativeById(string id)
        {
            return 
                _dbContext.Alternatives
                    .FirstOrDefault(a => a.Id == id);
        }
    }
}
