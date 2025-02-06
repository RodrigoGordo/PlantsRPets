using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Services
{
    /// <summary>
    /// Implementação do serviço de envio de e-mails utilizando a API do SendGrid.
    /// Responsável por enviar e-mails transacionais, como redefinição de palavra-passe ou notificações.
    /// </summary>
    public class SendGridEmailService : IEmailService
    {
        private readonly string _sendGridApiKey;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="SendGridEmailService"/> com a chave de API do SendGrid.
        /// </summary>
        /// <param name="configuration">Configuração da aplicação utilizada para aceder à chave de API do SendGrid.</param>
        public SendGridEmailService(IConfiguration configuration)
        {
            _sendGridApiKey = configuration["SendGrid:ApiKey"];
        }

        /// <summary>
        /// Envia um e-mail utilizando o serviço do SendGrid.
        /// </summary>
        /// <param name="toEmail">Endereço de e-mail do destinatário.</param>
        /// <param name="subject">Assunto do e-mail.</param>
        /// <param name="body">Conteúdo do e-mail, podendo ser texto simples ou HTML.</param>
        /// <exception cref="Exception">Lançada quando a chave de API está em falta ou o envio falha.</exception>
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
