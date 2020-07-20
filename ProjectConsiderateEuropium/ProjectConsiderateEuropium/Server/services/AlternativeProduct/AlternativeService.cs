using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ProjectConsiderateEuropium.Shared.Models;
using ProjectConsiderateEuropium.Shared.Models.ModelTypes;

namespace ProjectConsiderateEuropium.Server.services.AlternativeProduct
{
    public class AlternativeService
    {
        private readonly IAlternativeGetterService _alternativeGetterService;

        public AlternativeService(IAlternativeGetterService alternativeGetterService)
        {
            _alternativeGetterService = alternativeGetterService;
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


    }
}
