using EShop.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Service
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        private static string[] passwords;

        static PasswordValidator()
        {
            passwords = File.ReadAllLines("passwords.txt");
        }

        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            //Secret123$
            if (user.UserName != password && !passwords.Contains(password))
            {
                return Task.FromResult(IdentityResult.Success);
            }

            var error = new IdentityError
            {
                Code = "login_validation",
                Description = "Your password is not secure!"
            };
            return Task.FromResult(IdentityResult.Failed(error));
        }
    }
}
