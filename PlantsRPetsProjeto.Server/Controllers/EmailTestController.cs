using Microsoft.AspNetCore.Mvc;
using Quartz;
using PlantsRPetsProjeto.Server.Services;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [ApiController]
    [Route("api/email-test")]
    public class EmailTestController : ControllerBase
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IEmailService _emailService;
        public EmailTestController(ISchedulerFactory schedulerFactory, IEmailService emailService)

        {
            _schedulerFactory = schedulerFactory;
            _emailService = emailService;
        }

        [HttpPost("trigger-notification-job")]
        public async Task<IActionResult> TriggerNotificationJob()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey("SendNotificationEmail");

            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.TriggerJob(jobKey);
                return Ok("Notification job triggered successfully!");
            }
            else
            {
                return NotFound("Notification job not found.");
            }
        }

        [HttpPost("send-direct-email")]
        public async Task<IActionResult> SendDirectEmail()
        {
            await _emailService.SendEmailAsync("ruben.metin60@gmail.com", "Test Email", "Hello, this is a test.");
            return Ok("Test email sent!");
        }
    }
}
