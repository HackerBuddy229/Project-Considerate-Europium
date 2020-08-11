using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProjectConsiderateEuropium.Server.Data;
using ProjectConsiderateEuropium.Server.handlers;
using ProjectConsiderateEuropium.Server.Wrappers;
using ProjectConsiderateEuropium.Shared.Communication;
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
        CreationResult<Alternative> CreateAlternative(UserEntityCreationRequest<Alternative> alternative);
    }

    public class AlternativeService : IAlternativeService
    {
        private readonly IAlternativeGetterService _alternativeGetterService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IAlternativeCreationService _alternativeCreationService;
        private readonly IUserImageHandler _userImageHandler;

        public AlternativeService(IAlternativeGetterService alternativeGetterService,
            IAlternativeCreationService alternativeCreationService,
            ApplicationDbContext dbContext,
            IUserImageHandler userImageHandler)
        {
            _alternativeGetterService = alternativeGetterService;
            _dbContext = dbContext;
            _alternativeCreationService = alternativeCreationService;
            _userImageHandler = userImageHandler;
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

        public CreationResult<Alternative> CreateAlternative(UserEntityCreationRequest<Alternative> alternative)
        {
            //verify file
            if (!_userImageHandler.ValidateImage(alternative.Image))
                return new CreationResult<Alternative>{Errors = new List<string>{"Invalid Image"}};

            //get path
            alternative.Image = _userImageHandler.CreateUserImagePath(alternative.Image);
            alternative.NewEntity.Logo = alternative.Image.Path;


            //create product
            var result = _alternativeCreationService.CreateAlternative(alternative.NewEntity);

            //verify Entry
            if (!result.Succeeded)
                return new CreationResult<Alternative>{Errors = new List<string>{"bad product"}};

            //write Image
            try
            {
                _userImageHandler.WriteImage(alternative.Image);
            }
            catch (IOException ex)
            {
                _dbContext.Alternatives.Remove(result.Created);
                _dbContext.SaveChanges();

                return new CreationResult<Alternative>{Errors = new List<string>{$"Error writing Image {ex.Message}"}};
            }

            //return result
            return new CreationResult<Alternative>{Created = result.Created};
        }
    }
}
