using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ProjectConsiderateEuropium.Server.Data.Models;
using ProjectConsiderateEuropium.Server.External;
using ProjectConsiderateEuropium.Server.Wrappers;
using ProjectConsiderateEuropium.Shared.Models.Authentication;
using ProjectConsiderateEuropium.Shared.Models.Types;

namespace ProjectConsiderateEuropium.Server.Authentication.Services
{
    public interface ITokenCreationService
    {
        TokenSet CreateTokenSet(ApplicationIdentityUser user, bool keepMeLoggedIn = false);
    }

    public class TokenCreationService : ITokenCreationService
    {
        private readonly IJwtConfiguration _jwtConfiguration;

        public TokenCreationService(IJwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration;
        }
        public TokenSet CreateTokenSet(ApplicationIdentityUser user, bool keepMeLoggedIn = false)
        {
            var result = new TokenSet();

            var authenticationToken = CreateToken(user, TokenType.AuthenticationToken);
            if (!authenticationToken.Succeeded)
                return null;

            result.AuthenticationToken.AuthenticationBearerToken = authenticationToken.TokenCreated;
            result.AuthenticationToken.AuthenticationTokenExpires =
                DateTime.Now.AddHours(_jwtConfiguration.AuthenticationTokenValidTimeHours).ToString(CultureInfo.InvariantCulture);

            if (keepMeLoggedIn)
            {
                var refreshToken = CreateToken(user, TokenType.RefreshToken);
                if (!refreshToken.Succeeded)
                    return null;

                result.RefreshToken.RefreshBearerToken = refreshToken.TokenCreated;
                result.RefreshToken.RefreshTokenExpires =
                    DateTime.Now.AddDays(_jwtConfiguration.RefreshTokenValidTimeDays).ToString(CultureInfo.InvariantCulture);
            }

            return result;

        }

        public TokenResponse CreateToken(ApplicationIdentityUser user, TokenType tokenType)
        {
            var claims = CreateUserTokenClaims(user, tokenType);
            if (claims == null || !claims.Any()) 
                return new TokenResponse(){Errors = new List<string>(){"bad input, no claims possible"}};

            var credentials = SigningCredentials(tokenType);
            if (credentials == null)
                return new TokenResponse(){Errors = new List<string>(){"bad token type or configuration not setup"}};

            var specifications = CreateJwtSpecifications(credentials, claims, tokenType);

            var token = new JwtSecurityTokenHandler().WriteToken(specifications);
            return new TokenResponse(){TokenCreated = token};
        }

        private IEnumerable<Claim> CreateUserTokenClaims(ApplicationIdentityUser user, TokenType tokenType)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email), new Claim(nameof(TokenType), tokenType.ToString())
            };

            //token generation if refresh
            if (tokenType == TokenType.RefreshToken)
                claims.Add(new Claim(ApplicationClaimType.TokenGeneration.ToString(), user.RefreshTokenGeneration));

            return claims;
        }

        private JwtSecurityToken CreateJwtSpecifications(SigningCredentials signingCredentials,
            IEnumerable<Claim> claims, TokenType tokenType)
        {
            var result = new JwtSecurityToken(
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: tokenType switch
                {
                    TokenType.AuthenticationToken => DateTime.Now.AddHours(_jwtConfiguration
                        .AuthenticationTokenValidTimeHours),
                    TokenType.RefreshToken => DateTime.Now.AddDays(_jwtConfiguration.RefreshTokenValidTimeDays),
                    _ => throw new ArgumentNullException()
                },
                signingCredentials: signingCredentials
            );

            return result;
        }

        private SigningCredentials SigningCredentials(TokenType tokenType)
        {
            var secret = tokenType switch
            {
                TokenType.AuthenticationToken => _jwtConfiguration.AuthenticationSigningKey,
                TokenType.RefreshToken => _jwtConfiguration.RefreshSigningKey,
                _ => throw new ArgumentNullException()
            };
            var bytes = Encoding.UTF8.GetBytes(secret);
            var key = new SymmetricSecurityKey(bytes);

            var algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorithm);
            return signingCredentials;
        }
    }
}
