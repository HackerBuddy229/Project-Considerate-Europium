using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ProjectConsiderateEuropium.Server.Data.Models;
using ProjectConsiderateEuropium.Server.External;
using ProjectConsiderateEuropium.Shared.Models.Types;

namespace ProjectConsiderateEuropium.Server.Authentication.Services
{
    public interface ITokenValidationService
    {
        IEnumerable<Claim> GetTokenClaims(string token);
        bool ValidateRefreshToken(string token, ApplicationIdentityUser user);
    }
    public class TokenValidationService : ITokenValidationService
    {
        private readonly IJwtConfiguration _jwtConfiguration;

        public TokenValidationService(IJwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration;
        }


        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            var serializedToken = ReadJwtSecurityToken(token);
            return serializedToken.Claims;
        }

        private JwtSecurityToken ReadJwtSecurityToken(string token)
        {
            return string.IsNullOrWhiteSpace(token) ? 
                null : new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        private TokenValidationParameters RefreshTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtConfiguration.ValidIssuer,
                ValidAudience = _jwtConfiguration.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.RefreshSigningKey))
            };
        }

        public bool ValidateRefreshToken(string token, ApplicationIdentityUser user)
        {
            try
            {
                var principal = 
                    new JwtSecurityTokenHandler().ValidateToken(token, RefreshTokenValidationParameters(), out _);

                var generationClaim = principal.Claims
                    .FirstOrDefault(c => c.Type == ApplicationClaimType.TokenGeneration.ToString());

                return generationClaim != null && generationClaim.Value == user.RefreshTokenGeneration;
            }
            catch
            {
                return false;
            }
        }


    }
}
