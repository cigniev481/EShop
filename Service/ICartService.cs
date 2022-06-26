using EShop.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Service
{
    public interface ICartService
    {
        void Add(int id);
        void Remove(int id);
        IEnumerable<CartItemViewModel> GetProducts();
        void Clear();
    }
}
