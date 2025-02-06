namespace PlantsRPetsProjeto.Server.Services
{
    /// <summary>
    /// Interface para o serviço de envio de e-mails.
    /// Define o contrato para o envio assíncrono de e-mails na aplicação.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Envia um e-mail de forma assíncrona para o destinatário especificado.
        /// </summary>
        /// <param name="toEmail">Endereço de e-mail do destinatário.</param>
        /// <param name="subject">Assunto do e-mail.</param>
        /// <param name="body">Conteúdo do e-mail, podendo incluir HTML.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona de envio de e-mail.</returns>
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
