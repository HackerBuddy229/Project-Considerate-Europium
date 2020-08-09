using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectConsiderateEuropium.Server.Data.Models;

namespace ProjectConsiderateEuropium.Server.Authentication.Services
{
    public interface IUserManagementService
    {
        Task<ApplicationIdentityUser> GetUserByEmail(string email);
        Task<ApplicationIdentityUser> GetUserById(string id);

        Task<bool> VerifyUserCredentials(ApplicationIdentityUser user, string password);
        Task<bool> VerifyUserCredentials(string email, string password);

        Task ResetTokenGeneration(ApplicationIdentityUser user);
        Task ResetTokenGeneration(string email);
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationIdentityUser> _userManager;

        public UserManagementService(UserManager<ApplicationIdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationIdentityUser> GetUserByEmail(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }

        public async Task<ApplicationIdentityUser> GetUserById(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            return result;
        }


        public async Task<bool> VerifyUserCredentials(ApplicationIdentityUser user, string password)
        {
            var result = await UserPasswordIsValid(user, password);
            return result;
        }

        public async Task<bool> VerifyUserCredentials(string email, string password)
        {
            var user = await GetUserByEmail(email);

            var result = await VerifyUserCredentials(user, password);
            return result;
        }

        public async Task ResetTokenGeneration(ApplicationIdentityUser user)
        {
            user.RefreshTokenGeneration = Guid.NewGuid().ToString();
            await UpdateUser(user);
        }

        public async Task ResetTokenGeneration(string email)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
                return;

            user.RefreshTokenGeneration = Guid.NewGuid().ToString();
            await UpdateUser(user);
        }

        private async Task UpdateUser(ApplicationIdentityUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        private async Task<bool> UserPasswordIsValid(ApplicationIdentityUser user, string password)
        {
            var result = await _userManager.CheckPasswordAsync(user, password);
            return result;
        }
    }
}
