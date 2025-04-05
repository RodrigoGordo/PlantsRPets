using PlantsRPetsProjeto.Server.Models;
using Microsoft.EntityFrameworkCore;
using Quartz;
using PlantsRPetsProjeto.Server.Data;

namespace PlantsRPetsProjeto.Server.Services
{
    /// <summary>
    /// Responsável por enviar e-mails com notificações não lidas a utilizadores, de acordo com a sua frequência de notificação definida.
    /// Implementa a interface <see cref="IJob"/> para ser executado periodicamente através do Quartz.NET.
    /// </summary>
    public class SendNotificationEmail : IJob
    {
        private readonly IEmailService _emailService;
        private readonly PlantsRPetsProjetoServerContext _dbContext;

        /// <summary>
        /// Construtor do job de envio de notificações por e-mail.
        /// </summary>
        /// <param name="emailService">Serviço responsável pelo envio dos e-mails.</param>
        /// <param name="dbContext">Contexto da base de dados da aplicação.</param>
        public SendNotificationEmail(IEmailService emailService, PlantsRPetsProjetoServerContext dbContext)
        {
            _emailService = emailService;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Método principal do job, executado automaticamente segundo o agendamento configurado.
        /// Envia e-mails aos utilizadores que têm notificações por ler, de acordo com a frequência definida no seu perfil.
        /// </summary>
        /// <param name="context">Contexto de execução do Quartz.</param>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("🔹 SendNotificationEmail job started.");
                var today = DateTime.UtcNow;

                var users = await _dbContext.Users
                    .Where(u => u.NotificationFrequency != User.EmailFrequency.Never)
                    .ToListAsync();

                foreach (var user in users)
                {
                    //if (!ShouldSendEmail(user.NotificationFrequency, user.Id, today))
                    //    continue;

                    var unreadNotifications = await _dbContext.UserNotifications
                        .Where(n => n.UserId == user.Id && !n.isRead)
                        .Include(n => n.Notification)
                        .ToListAsync();

                    if (unreadNotifications.Any())
                    {
                        var emailBody = string.Join("<br>", unreadNotifications.Select(n => n.Notification.Message));

                        Console.WriteLine($"📧 Sending email to: {user.Email}");
                        await _emailService.SendEmailAsync(user.Email, "Your Notifications", emailBody);
                        Console.WriteLine($"✅ Email sent successfully to {user.Email}");
                    }
                    else
                    {
                        Console.WriteLine($"ℹ️ No unread notifications for {user.Email}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in SendNotificationEmail job: {ex.Message}");
            }

        }

        /// <summary>
        /// Verifica se é apropriado enviar um e-mail ao utilizador com base na sua frequência definida.
        /// (Atualmente não utilizado — comentado no método <see cref="Execute"/>).
        /// </summary>
        /// <param name="frequency">Frequência de notificação definida pelo utilizador (Diária, Semanal, Mensal).</param>
        /// <param name="userId">Identificador do utilizador.</param>
        /// <param name="today">Data atual.</param>
        /// <returns>True se a notificação estiver dentro do intervalo de envio; caso contrário, false.</returns>
        private bool ShouldSendEmail(User.EmailFrequency frequency, string userId, DateTime today)
        {
            var lastSent = _dbContext.UserNotifications
                .Where(n => n.UserId == userId && n.isRead)
                .OrderByDescending(n => n.ReceivedDate)
                .Select(n => n.ReceivedDate)
                .FirstOrDefault();

            return frequency switch
            {
                User.EmailFrequency.Daily => lastSent.Date != today.Date,
                User.EmailFrequency.Weekly => (today - lastSent).TotalDays >= 7,
                User.EmailFrequency.Monthly => (today - lastSent).TotalDays >= 30,
                _ => false
            };
        }
    }

}
