using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Server.Wrappers;
using ProjectConsiderateEuropium.Shared.Models;
using System;
using System.Collections.Generic;

namespace ProjectConsiderateEuropium.Server.services.AlternativeProduct
{
    public interface IAlternativeCreationService
    {
        public CreationResult<Alternative> CreateAlternative(Alternative alternative);
        public bool IsValidAlternative(Alternative alternative);
    }
    public class AlternativeCreationService : IAlternativeCreationService
    {
        private readonly ApplicationDbContext _dbContext;

        public AlternativeCreationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public CreationResult<Alternative> CreateAlternative(Alternative alternative)
        {
            if (!IsValidAlternative(alternative))
                return new CreationResult<Alternative> { Errors = new List<string> { "Invalid alternative" } };

            _dbContext.BeginTransaction();

            _dbContext.Alternatives.Add(alternative);

            try
            {
                _dbContext.Commit();
                return new CreationResult<Alternative> { Created = alternative };
            } catch (Exception ex)
            {
                _dbContext.Rollback();
                return new CreationResult<Alternative> { Errors = new List<string> { $"Db error: {ex.Message}" } };
            }
        }

        public bool IsValidAlternative(Alternative alternative)
        {
            //not null
            if (alternative != null)
                if (!string.IsNullOrEmpty(alternative.Id))
                    if (!string.IsNullOrEmpty(alternative.Logo))
                        if (!string.IsNullOrEmpty(alternative.AccentColor))
                            if (!string.IsNullOrEmpty(alternative.Designation))
                                if (!string.IsNullOrEmpty(alternative.Contributor))
                                    return true;
            return false;
        }
    }
}
