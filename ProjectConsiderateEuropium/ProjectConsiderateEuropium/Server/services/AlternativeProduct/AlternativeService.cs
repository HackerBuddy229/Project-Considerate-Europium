using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Server.Wrappers;
using ProjectConsiderateEuropium.Shared.Filter;
using ProjectConsiderateEuropium.Shared.Models;
using ProjectConsiderateEuropium.Shared.Models.ModelTypes;
using ProjectConsiderateEuropium.Shared.Wrappers;

namespace ProjectConsiderateEuropium.Server.services.AlternativeProduct
{
    public interface IAlternativeService
    {
        Alternative GetAlternative(string identifier, IdentificationType identificationType);
        IEnumerable<Alternative> GetNewestAlternatives(int amount);
        IEnumerable<Alternative> GetOldestAlternatives(int amount);
        PagedResponse<IEnumerable<Alternative>> GetPagedAlternativesByNewest(PaginationFilter filter, string route);
        CreationResult<Alternative> CreateAlternative(Alternative alternative); //debug
    }

    public class AlternativeService : IAlternativeService
    {
        private readonly IAlternativeGetterService _alternativeGetterService;
        private readonly ApplicationDbContext _dbContext;

        public AlternativeService(IAlternativeGetterService alternativeGetterService, 
            ApplicationDbContext dbContext)
        {
            _alternativeGetterService = alternativeGetterService;
            _dbContext = dbContext;
        }

        public Alternative GetAlternative(string identifier, IdentificationType identificationType)
        {
            return identificationType switch
            {
                IdentificationType.Id =>
                _alternativeGetterService.GetAlternativeById(identifier),

                IdentificationType.Designation =>
                _alternativeGetterService.GetAlternativeByDesignation(identifier),

                IdentificationType.OrganizationIdentifier =>
                _alternativeGetterService.GetAlternativeByDesignation(identifier),

                _ => null
            };
        }

        public IEnumerable<Alternative> GetNewestAlternatives(int amount)
        {
            return _alternativeGetterService.GetAlternativesByDescendingCreated(amount);
        }

        public IEnumerable<Alternative> GetOldestAlternatives(int amount)
        {
            return _alternativeGetterService.GetAlternativesByAscendingCreated(amount);
        }

        public PagedResponse<IEnumerable<Alternative>> GetPagedAlternativesByNewest(PaginationFilter filter, string route)
        {
            return _alternativeGetterService.GetPagedAlternativesByDescendingCreated(filter, route);
        }

        public CreationResult<Alternative> CreateAlternative(Alternative alternative)
        {
            if (alternative == null)
                return new CreationResult<Alternative>(){Errors = new List<string>(){"null input"}};

            _dbContext.BeginTransaction();

            _dbContext.Alternatives.Add(alternative);

            try
            {
                _dbContext.Commit();
            }
            catch (Exception ex)
            {
                _dbContext.Rollback();
                return new CreationResult<Alternative>(){Errors = new List<string>(){$"dbError: {ex.Message}"}};
            }

            return new CreationResult<Alternative>(){Created = alternative};
        }
    }
}
