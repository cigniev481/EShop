using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EShop.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using EShop.Service;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Identity;
using EShop.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly EShopDbContext context;
        private readonly ICartService cartService;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public HomeController(
            EShopDbContext context, 
            ICartService cartService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            this.context = context;
            this.cartService = cartService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        { 
            //ViewBag.Date = HttpContext.Session.GetString("Date");
            ViewBag.Lang = CultureInfo.CurrentCulture;
            ViewBag.UILang = CultureInfo.CurrentUICulture;
            return View(context.Products);
        }

        public IActionResult AddToCart(int id, string returnUrl)
        {
            cartService.Add(id);

            if (returnUrl == null)
                return RedirectToAction("Index", "Home");
            else
                return Redirect(returnUrl);
        }

        //[Authorize]
        public async Task<IActionResult> Privacy()
        {
            //HttpContext.Session.SetString("Date", DateTime.Now.ToString());

            //var user = await userManager.FindByNameAsync(User.Identity.Name);
            //if (!User.HasClaim("privacy_accepted", "true"))
            //{
            //    var claim = new Claim("privacy_accepted", "true");
            //    await userManager.AddClaimAsync(user, claim);
            //    await signInManager.SignInAsync(user, true);
            //}
            return View();
        }

        [Authorize(Policy = "test_policy")]
        public IActionResult Test()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
