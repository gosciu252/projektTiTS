using AspNetIdentitySample.Data;
using AspNetIdentitySample.Models.Database;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentitySample.Services
{
    /// <summary>
    /// Klasa służąca do operacji na użytkownikach w bazie danych.
    /// </summary>
    public class UsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> SetTokenGeneratedForUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.TokenGenerated = true;
            user.GenerationTimestamp = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
