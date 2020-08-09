using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectConsiderateEuropium.Shared.Communication;
using ProjectConsiderateEuropium.Shared.Models.Authentication;

namespace ProjectConsiderateEuropium.Server.Authentication.Services
{
    [Route("api/Authenticate")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("by-applicationUser")]
        public async Task<IActionResult> AuthenticateByApplicationUser(AuthenticationRequest request)
        {
            var result = await _authenticationService.AuthenticateApplicationUser(request);
            if (result.Succeeded)
                return Ok(result.Tokens);
            return BadRequest("Bad Input");
        }

        [HttpPost("by-refreshToken")]
        public async Task<IActionResult> AuthenticateByRefreshToken(RefreshToken token)
        {
            var result = await _authenticationService.AuthenticateRefreshToken(token);
            if (result.Succeeded)
                return Ok(result.Tokens);
            return BadRequest("Bad Token");
        }

    }
}
