using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ProjectConsiderateEuropium.Server.Data.Models;
using ProjectConsiderateEuropium.Shared.Models.Authentication;

namespace ProjectConsiderateEuropium.Server.Authentication.Services
{

    public interface ITokenService
    {
        TokenSet CreateAuthenticationTokenSet(ApplicationIdentityUser user, bool keepMeLoggedIn = false);

        bool ValidateToken(RefreshToken token, ApplicationIdentityUser user);
        IEnumerable<Claim> GetTokenClaims(string token);
    }

    public class TokenService : ITokenService
    {
        private readonly ITokenCreationService _tokenCreationService;
        private readonly ITokenValidationService _tokenValidationService;

        public TokenService(ITokenCreationService tokenCreationService, ITokenValidationService tokenValidationService)
        {
            _tokenCreationService = tokenCreationService;
            _tokenValidationService = tokenValidationService;
        }


        public TokenSet CreateAuthenticationTokenSet(ApplicationIdentityUser user, bool keepMeLoggedIn = false)
        {
            return _tokenCreationService.CreateTokenSet(user, keepMeLoggedIn);
        }

        public bool ValidateToken(RefreshToken token, ApplicationIdentityUser user)
        {
            return _tokenValidationService.ValidateRefreshToken(token.RefreshBearerToken, user);
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            return _tokenValidationService.GetTokenClaims(token);
        }
    }
}
