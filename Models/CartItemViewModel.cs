using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models
{
    public class CartItemViewModel
    {
        public Product Product { get; set; }
        public int Amount { get; set; }
    }
}
