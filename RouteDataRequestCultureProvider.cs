using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop
{
    public class RouteDataRequestCultureProvider : RequestCultureProvider
    {
        public int IndexOfCulture = 1;
        public int IndexofUICulture = 1;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            // /ru/Home/Index => ["", "ru", "Home", "Index"]
            var culture = httpContext.Request.Path.Value.Split('/')[IndexOfCulture]?.ToString();

            if (culture == null)
                return NullProviderCultureResult;

            var providerResultCulture = new ProviderCultureResult(culture, culture);

            return Task.FromResult(providerResultCulture);
        }
    }
}
