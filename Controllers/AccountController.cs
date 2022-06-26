using EShop.Models;
using EShop.Models.Identity;
using EShop.Service;
using EShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailSender emailSender;

        //UserManager
        //RoleManager
        //SingInManager
        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Year = model.Year
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, token = token },
                        protocol: HttpContext.Request.Scheme);
                    await emailSender.SendAsync(user.Email, "Please confirm your registration", callbackUrl);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var user = await userManager.FindByNameAsync(model.Login);
            if (user != null)
            {
                if (await userManager.IsEmailConfirmedAsync(user))
                {
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrWhiteSpace(returnUrl))
                            return Redirect(returnUrl);

                        return RedirectToAction("Index", "Home");
                    }
                    else if (result.IsLockedOut) ModelState.AddModelError("loginForm", "Your account is locked");
                    else ModelState.AddModelError("loginForm", "Incorrect username or password");

                    //if (await userManager.CheckPasswordAsync(user, model.Password))
                    //{
                    //    await signInManager.SignInAsync(user, model.RememberMe);

                    //    if (!string.IsNullOrWhiteSpace(returnUrl))
                    //        return Redirect(returnUrl);

                    //    return RedirectToAction("Index", "Home");
                    //}
                    //else ModelState.AddModelError("loginForm", "Incorrect username or password");
                }
                else ModelState.AddModelError("loginForm", "Please confirm your email");
            }
            else ModelState.AddModelError("loginForm", "Incorrect username or password");

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Email confirmed!";
                    return View();
                }
            }
            ViewBag.Message = "Error!";
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
           
            if (info == null)
            { 
                ModelState.AddModelError("login_fail", "Login failed!");
                return RedirectToAction("Login", "Account");
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, info.AuthenticationProperties.IsPersistent);
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("login_fail", "Your account is locked!");
                return RedirectToAction("Login", "Account");
            }
            else if (info.Principal.HasClaim(x => x.Type == ClaimTypes.Email))
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? "Unknown";
                var user = new AppUser
                {
                    Email = email,
                    UserName = email,
                    FullName = name,
                    Year = 2000,
                    EmailConfirmed = true
                };
                var createResult = await userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    var addResult = await userManager.AddLoginAsync(user, info);
                    if (addResult.Succeeded)
                    {
                        await signInManager.SignInAsync(user, info.AuthenticationProperties.IsPersistent);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("login_fail", "Error!!!");
                return RedirectToAction("Login", "Account");
            }

            throw new Exception("ERROR!!!");
        }
    }
}
