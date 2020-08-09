using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using ProjectConsiderateEuropium.Shared.Communication;
using ProjectConsiderateEuropium.Shared.Models.Authentication;

namespace ProjectConsiderateEuropium.Server.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateApplicationUser(AuthenticationRequest request);
        Task<AuthenticationResponse> AuthenticateRefreshToken(RefreshToken token);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserManagementService _userManagementService;

        public AuthenticationService(ITokenService tokenService,
            IUserManagementService userManagementService)
        {
            _tokenService = tokenService;
            _userManagementService = userManagementService;
        }
        public async Task<AuthenticationResponse> AuthenticateApplicationUser(AuthenticationRequest request)
        {
            var user = await _userManagementService.GetUserByEmail(request.Email);
            var isValid = await _userManagementService.VerifyUserCredentials(user, request.Password);

            if (!isValid)
                return new AuthenticationResponse();

            var tokenSet = _tokenService.CreateAuthenticationTokenSet(user, request.KeepMeLoggedIn);
            return new AuthenticationResponse(){Tokens = tokenSet};
        }

        public async Task<AuthenticationResponse> AuthenticateRefreshToken(RefreshToken token)
        {
            //get token claims
            var claims = _tokenService.GetTokenClaims(token.RefreshBearerToken);
            if (!claims.Any())
                return new AuthenticationResponse();

            //get email
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            if (string.IsNullOrWhiteSpace(email))
                return new AuthenticationResponse();

            //get user
            var user = await _userManagementService.GetUserByEmail(email);
            if (user == null)
                return new AuthenticationResponse();

            //validate token
            var tokenIsValid = _tokenService.ValidateToken(token, user);
            if (!tokenIsValid) 
                return new AuthenticationResponse();

            //create tokenSet
            var tokenSet = _tokenService.CreateAuthenticationTokenSet(user, true);

            //return new tokenSet
            return new AuthenticationResponse{Tokens = tokenSet};
        }
    }
}
