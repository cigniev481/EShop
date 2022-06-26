using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Service
{
    public class EmailSender : IEmailSender
    {
        public async Task SendAsync(string email, string subject, string content)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("EShop Admin", "postmaster@sandboxb86b0573399544a4bf1207c30ba638de.mailgun.org"));
            emailMessage.To.Add(new MailboxAddress("EShop Customer", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = content
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mailgun.org", 587, false);
                await client.AuthenticateAsync("postmaster@sandboxb86b0573399544a4bf1207c30ba638de.mailgun.org", "9d06c1845182b889107aba1302b0eea9-2b0eef4c-f746d3be");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
