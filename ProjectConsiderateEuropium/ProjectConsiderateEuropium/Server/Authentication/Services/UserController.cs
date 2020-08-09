using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectConsiderateEuropium.Server.Authentication.Services
{
    [Route("api/manage-user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }


        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> LogoutUser() //double Check this!!!
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
                return BadRequest("server error... sry :(");

            var email = emailClaim.Value;
            try
            {
                await _userManagementService.ResetTokenGeneration(email);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}
