using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Service
{
    public interface IEmailSender
    {
        Task SendAsync(string email, string subject, string content);
    }
}
