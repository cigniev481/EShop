using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EShop.Authorization;
using EShop.Models;
using EShop.Models.Identity;
using EShop.Models.Validators;
using EShop.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPasswordValidator<AppUser>, PasswordValidator>();

            services.AddDbContext<EShopDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequireDigit = false;
                //options.Password.RequireUppercase = false;
                //options.Password.RequireLowercase = false;
                //options.Password.RequiredLength = 4;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequiredUniqueChars = 0;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.";
            })
            .AddEntityFrameworkStores<EShopDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                //options.LoginPath = "/Home/Privacy";
                //options.AccessDeniedPath = "/Home/Privacy";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("test_policy", builder =>
                {
                    builder.RequireClaim("privacy_accepted", "true");
                    builder.Requirements.Add(new AgeRequirement { Age = 20 });
                });
            });

            services.AddTransient<AuthorizationHandler<AgeRequirement>, AgeAuthorizationHandler>();

            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = Configuration["FacebookLogin:AppId"];
                    options.AppSecret = Configuration["FacebookLogin:AppSecret"];
                });

            //-----------------------------------------------------------

            services
                .AddControllersWithViews()
                .AddFluentValidation();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IValidator<Product>, ProductValidator>();
            services.AddTransient<IValidator<Category>, CategoryValidator>();
            services.AddTransient<IValidator<Order>, OrderValidator>();

            services.AddLocalization(optons => 
            {
                optons.ResourcesPath = "Resources";
            });

            services.AddHttpContextAccessor();

            services.AddSession(options => 
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ru"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new[]
                { 
                    new RouteDataRequestCultureProvider{
                        IndexOfCulture=1,
                        IndexofUICulture=1
                    }
                }
        });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //    endpoints.MapControllerRoute(
                //        name: "makeorder",
                //        pattern: "MakeOrder/{id:int}",
                //        defaults: new { controller = "Orders", action = "Create" });

                //endpoints.MapControllerRoute(
                //    name: "makeorder",
                //    pattern: "MakeOrder/{*catchall}",
                //    defaults: new { controller = "Orders", action = "Create" });

                //endpoints.MapControllerRoute(
                //    name: "areas",
                //    pattern: "{culture=en}/{area:exists}/{controller=Products}/{action=Index}/{id?}");

                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{culture=en}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Products}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
