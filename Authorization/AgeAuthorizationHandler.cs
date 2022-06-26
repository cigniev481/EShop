using EShop.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Authorization
{
    public class AgeAuthorizationHandler : AuthorizationHandler<AgeRequirement>
    {
        private readonly UserManager<AppUser> userManager;

        public AgeAuthorizationHandler(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user != null && DateTime.Now.Year - user.Year >= requirement.Age)
                context.Succeed(requirement);
            else 
                context.Fail();
        }
    }
}
