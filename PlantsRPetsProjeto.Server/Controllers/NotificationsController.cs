using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public NotificationController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserNotification>>> GetUserNotifications()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var notifications = await _context.UserNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId)
                .ToListAsync();

            return Ok(notifications);
        }

        [HttpPost("send/{notificationId}")]
        public async Task<IActionResult> SendNotification(int notificationId)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var notification = await _context.Notification.FindAsync(notificationId);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }

            var userNotification = new UserNotification
            {
                UserId = userId,
                NotificationId = notificationId,
                ReceivedDate = DateTime.UtcNow,
                isRead = false
            };

            _context.UserNotifications.Add(userNotification);
            await _context.SaveChangesAsync();

            return Ok("Notification sent to user.");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _context.UserNotifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            _context.UserNotifications.Remove(notification);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var userNotification = await _context.UserNotifications
                .FirstOrDefaultAsync(un => un.UserNotificationId == id && un.UserId == userId);

            if (userNotification == null)
            {
                return NotFound();
            }

            userNotification.isRead = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("unread")]
        public async Task<ActionResult<IEnumerable<UserNotification>>> GetUnreadNotifications()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var unreadNotifications = await _context.UserNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId && !un.isRead)
                .ToListAsync();

            return Ok(unreadNotifications);
        }

        [HttpPut("email-frequency/{frequencyId}")]
        public async Task<IActionResult> UpdateEmailFrequency(int frequencyId)
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!Enum.IsDefined(typeof(User.EmailFrequency), frequencyId))
            {
                return BadRequest("Invalid email frequency. Valid values: 0 (Never), 1 (Daily), 2 (Weekly), 3 (Monthly).");
            }

            user.NotificationFrequency = (User.EmailFrequency)frequencyId;
            await _context.SaveChangesAsync();

            return Ok($"Email frequency updated to {user.NotificationFrequency}.");
        }


    }
}