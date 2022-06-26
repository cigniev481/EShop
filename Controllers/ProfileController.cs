using EShop.Models.Identity;
using EShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
       
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var model = new ProfileViewModel
            {
                Email = user.Email,
                Fullname = user.FullName,
                Year = user.Year
            };

            return View(model);
        }
    }
}
