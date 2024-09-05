using Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        public readonly IConfiguration _configuration;

        public SmtpEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential("", "");
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(""),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
    }
}
