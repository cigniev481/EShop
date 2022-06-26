using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Authorization
{
    public class AgeRequirement : IAuthorizationRequirement
    {
        public int Age { get; set; }
    }
}
