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
    /// <summary>
    /// Controlador responsável pela gestão de notificações dos utilizadores, incluindo envio, leitura, remoção e frequência de email.
    /// Requer autenticação do utilizador para acesso aos endpoints.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        /// <summary>
        /// Construtor que injeta o contexto da base de dados.
        /// </summary>
        /// <param name="context">Contexto da base de dados da aplicação.</param>
        public NotificationController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as notificações de um utilizador autenticado.
        /// </summary>
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

        /// <summary>
        /// Envia uma notificação específica para o utilizador autenticado.
        /// </summary>
        /// <param name="notificationId">ID da notificação a enviar.</param>
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


        /// <summary>
        /// Elimina uma notificação do utilizador.
        /// </summary>
        /// <param name="id">ID da notificação do utilizador.</param>
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

        /// <summary>
        /// Marca uma notificação como lida.
        /// </summary>
        /// <param name="id">ID da notificação do utilizador.</param>
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

        /// <summary>
        /// Obtém todas as notificações não lidas do utilizador autenticado.
        /// </summary>
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

        /// <summary>
        /// Atualiza a frequência com que o utilizador deseja receber emails com notificações.
        /// </summary>
        /// <param name="frequencyId">Valor da frequência (0: Never, 1: Daily, 2: Weekly, 3: Monthly).</param>
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

        /// <summary>
        /// Obtém a frequência de envio de emails configurada pelo utilizador.
        /// </summary>
        [HttpGet("email-frequency")]
        public async Task<ActionResult<int>> GetUserEmailFrequency()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var frequency = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.NotificationFrequency)
                .SingleOrDefaultAsync();

            return Ok(frequency);
        }


    }
}