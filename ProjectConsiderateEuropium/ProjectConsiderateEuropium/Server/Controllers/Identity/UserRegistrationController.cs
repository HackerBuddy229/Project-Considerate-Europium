using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectConsiderateEuropium.Server.services.ApplicationUser;
using ProjectConsiderateEuropium.Shared.Communication;

namespace ProjectConsiderateEuropium.Server.Controllers.Identity
{
    [Route("api/register-user")]
    [ApiController]
    public class UserRegistrationController : ControllerBase
    {
        private readonly IUserRegistrationService _userRegistrationService;

        public UserRegistrationController(IUserRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationRequest request)
        {
            var result = await _userRegistrationService.CreateNewUser(request);

            if (result.Succeeded)
                return Ok(result.Request);

            return BadRequest(result.Errors.FirstOrDefault() switch
            {
                "Not a valid request" => "The username or email you have provided are not available",
                "Server Error" => "there has been a server error",
                _ => "something has gone wrong..."
            });
        }
    }
}
