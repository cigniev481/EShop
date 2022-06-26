using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Models.Identity;
using EShop.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var container = host.Services.CreateScope())
            {
                var userManager = container.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = container.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var configuration = container.ServiceProvider.GetRequiredService<IConfiguration>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);
                }

                var adminUsername = configuration["AdminAccount:Login"];
                var adminPassword = configuration["AdminAccount:Password"];

                var user = await userManager.FindByNameAsync(adminUsername);
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = adminUsername,
                        Email = adminUsername,
                        FullName = "Admin",
                        Year = 1994,
                        EmailConfirmed = true,
                        LockoutEnabled = false
                    };
                    var result = await userManager.CreateAsync(user, adminPassword);
                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);

                    result = await userManager.AddToRoleAsync(user, "Admin");
                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
