using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Controllers;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using Quartz;
using System.Text;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Services
{
    public class PlantNotificationService
    {
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly IEmailService _emailService;

        public PlantNotificationService(PlantsRPetsProjetoServerContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task CheckAndNotifyAsync()
        {
            var plantations = await _context.PlantationPlants
                .Include(pp => pp.ReferencePlant)
                .Include(pp => pp.ReferencePlantation)
                    .ThenInclude(p => p.User)
                .ToListAsync();

            foreach (var pp in plantations)
            {
                var user = pp.ReferencePlantation.User;
                if (user == null || string.IsNullOrWhiteSpace(user.Email)) continue;

                var (canWater, _) = PlantingAdvisor.CanWater(pp);
                var canHarvest = pp.HarvestDate.HasValue && pp.HarvestDate.Value.Date <= DateTime.UtcNow.Date;


                if (!canWater && !canHarvest) continue;

                if (canHarvest)
                {
                    var harvestNotification = _context.Notification.FirstOrDefault(n => n.Type == "Harvest Alert");
                    if (harvestNotification != null)
                    {
                        var existingHarvestNotification = await _context.UserNotifications
                            .FirstOrDefaultAsync(un => un.UserId == user.Id && un.NotificationId == harvestNotification.NotificationId);

                        if (existingHarvestNotification == null)
                        {
                            var notificationUser = new UserNotification
                            {
                                UserId = user.Id,
                                NotificationId = harvestNotification.NotificationId,
                                isRead = false,
                                ReceivedDate = DateTime.UtcNow
                            };

                            _context.UserNotifications.Add(notificationUser);
                        }
                    }
                }

                if (canWater)
                {
                    var waterNotification = _context.Notification.FirstOrDefault(n => n.Type == "Water Reminder");
                    if (waterNotification != null)
                    {
                        var existingWaterNotification = await _context.UserNotifications
                            .FirstOrDefaultAsync(un => un.UserId == user.Id && un.NotificationId == waterNotification.NotificationId);

                        if (existingWaterNotification == null)
                        {
                            var notificationUser = new UserNotification
                            {
                                UserId = user.Id,
                                NotificationId = waterNotification.NotificationId,
                                isRead = false,
                                ReceivedDate = DateTime.UtcNow
                            };

                            _context.UserNotifications.Add(notificationUser);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

    }

    public class DailyNotificationJob : IJob
    {
        private readonly PlantNotificationService _notificationService;

        public DailyNotificationJob(PlantNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _notificationService.CheckAndNotifyAsync();
        }
    }
}
