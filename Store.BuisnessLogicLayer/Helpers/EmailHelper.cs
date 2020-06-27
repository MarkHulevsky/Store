using Microsoft.Extensions.Configuration;
using Store.BuisnessLogic.Helpers.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Helpers
{
    public class EmailHelper: IEmailHalper
    {
        private readonly IConfiguration _configuration;
        
        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(string email, string subject, string body)
        {
            var emailSettingsSection = _configuration.GetSection("EmailSettings");
            var senderEmail = emailSettingsSection["SenderEmail"];
            var senderName = emailSettingsSection["SenderName"];
            var senderPassword = emailSettingsSection["SenderPassword"];

            var fromAddress = new MailAddress(senderEmail, senderName);
            var toAddress = new MailAddress(email);
            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            var stmpSettingsSection = _configuration.GetSection("StmpClientSettings");

            var smtpClient = new SmtpClient
            {
                Host = stmpSettingsSection["Host"],
                Port = int.Parse(stmpSettingsSection["Port"]),
                UseDefaultCredentials = bool.Parse(stmpSettingsSection["UseDefaultCredentials"]),
                DeliveryMethod = (SmtpDeliveryMethod)int.Parse(stmpSettingsSection["DeliveryMethod"]),
                EnableSsl = bool.Parse(stmpSettingsSection["EnableSsl"]),
                Credentials = new NetworkCredential(fromAddress.Address, senderPassword)
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
