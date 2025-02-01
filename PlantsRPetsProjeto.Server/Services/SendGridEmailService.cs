using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly string _sendGridApiKey;

        public SendGridEmailService(IConfiguration configuration)
        {
            _sendGridApiKey = configuration["SendGrid:ApiKey"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(_sendGridApiKey))
                throw new Exception("SendGrid API Key is missing. Please check your configuration.");

            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("plantsrpets@outlook.com", "PlantsRPets");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to send email. Status Code: {response.StatusCode}");
            }
        }
    }
}
