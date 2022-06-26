using EShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly EShopDbContext context;

        public CartViewComponent(EShopDbContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Product> products = new List<Product>();
            if (HttpContext.Session.Keys.Contains("cart"))
            {
                var cart = JsonConvert
                    .DeserializeObject<Dictionary<int, int>>(HttpContext.Session.GetString("cart"));
                ViewBag.Count = cart.Sum(x => x.Value);
                products = context.Products.Where(x => cart.Keys.Contains(x.Id));
                
                ViewBag.ProductCount = cart;
                ViewBag.Total = products.Sum(x => x.Price * cart[x.Id]);
            }
            else
                ViewBag.Count = 0;


            return View(products);
        }
    }
}
