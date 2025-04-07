using PlantsRPetsProjeto.Server.Models;
using Microsoft.EntityFrameworkCore;
using Quartz;
using PlantsRPetsProjeto.Server.Data;

namespace PlantsRPetsProjeto.Server.Services
{
    public class SendNotificationEmail : IJob
    {
        private readonly IEmailService _emailService;
        private readonly PlantsRPetsProjetoServerContext _dbContext;

        public SendNotificationEmail(IEmailService emailService, PlantsRPetsProjetoServerContext dbContext)
        {
            _emailService = emailService;
            _dbContext = dbContext;
        }


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
                    if (!ShouldSendEmail(user.NotificationFrequency, user.Id, today))
                        continue;

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
