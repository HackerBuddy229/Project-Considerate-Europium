using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectConsiderateEuropium.Server.Data.Models;
using ProjectConsiderateEuropium.Shared.Communication;

namespace ProjectConsiderateEuropium.Server.services.ApplicationUser
{
    public interface IUserRegistrationService
    {
        Task<RegistrationResponse> CreateNewUser(RegistrationRequest request);
    }


    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public UserRegistrationService(UserManager<ApplicationIdentityUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<RegistrationResponse> CreateNewUser(RegistrationRequest request)
        {
            if (!await IsValidRequest(request))
                return new RegistrationResponse{Errors = new List<string>{"Not a valid request"}};

            try
            {
                var user = new ApplicationIdentityUser
                {
                    Email = request.Email,
                    UserName = request.UserName
                };
                var result = await _userManager.CreateAsync(user, request.Password);

                return result.Succeeded ? 
                    new RegistrationResponse{Request = request} : new RegistrationResponse { Errors = new List<string> {"Server Error"} };
            }
            catch (Exception e)
            {
                return new RegistrationResponse{Errors = new List<string>{$"Server Error :{e.Message}"}};
            }
        }

        private async Task<bool> IsValidRequest(RegistrationRequest request)
        {
            var usernameIsTaken = await _userManager.Users.AnyAsync(u => u.UserName == request.UserName);
            if (usernameIsTaken)
                return false;

            var emailIsUsed = await _userManager.Users.AnyAsync(u => u.Email == request.Email);
            if (emailIsUsed)
                return false;


            return true;
        }
    }
}
