using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectConsiderateEuropium.Client;
using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Shared.Models;

namespace ProjectConsiderateEuropium.Server.services.AlternativeProduct
{
    public interface IAlternativeGetterService
    {
        Alternative GetAlternativeById(string id);
        Alternative GetAlternativeByDesignation(string designation);
        Alternative GetAlternativeByOrganizationIdentifier(string organizationIdentifier);

    }

    public class AlternativeGetterService : IAlternativeGetterService
    {
        private readonly ApplicationDbContext _dbContext;

        public AlternativeGetterService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public Alternative GetAlternativeById(string id)
        {
            return 
                _dbContext.Alternatives
                    .FirstOrDefault(a => a.Id == id);
        }
    }
}
