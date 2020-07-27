using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ProjectConsiderateEuropium.Server.External
{
    public interface IJwtConfiguration
    {
        string ValidAudience { get; set; }
        string ValidIssuer { get; set; }
        string RefreshSigningKey { get; set; }
        string AuthenticationSigningKey { get; set; }
        public int RefreshTokenValidTimeDays { get; set; }
        public int AuthenticationTokenValidTimeHours { get; set; }
    }

    public class JwtConfiguration : IJwtConfiguration
    {
        private readonly IConfiguration _configuration;

        public JwtConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;

            ValidAudience = _configuration.GetSection("Jwt").GetValue<string>("ValidAudience");
            ValidIssuer = _configuration.GetSection("Jwt").GetValue<string>("ValidIssuer");

            RefreshTokenValidTimeDays = _configuration.GetSection("Jwt").GetValue<int>("RefreshValidTimeDays");
            AuthenticationTokenValidTimeHours = _configuration.GetSection("Jwt").GetValue<int>("AuthenticationValidTimeHours");

            RefreshSigningKey = _configuration["Jwt:RefreshSigningKey"]; //TODO: make sure secrets work in non dev
            AuthenticationSigningKey = _configuration["Jwt:AuthenticationSigningKey"];
        }

        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }

        public string RefreshSigningKey { get; set; }
        public string AuthenticationSigningKey { get; set; }
        public int RefreshTokenValidTimeDays { get; set; }
        public int AuthenticationTokenValidTimeHours { get; set; }
    }
}
