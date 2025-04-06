using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PlantsRPetsProjeto.Server.Controllers;
using PlantsRPetsProjeto.Server.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PlantsRPetsProjeto.Server.Data;

namespace PlantsRPetsProjeto.Tests.xUnitTests.ControllerTests
{
    public class NotificationControllerTests
    {
        private NotificationController GetControllerWithContext(string userId, PlantsRPetsProjetoServerContext context)
        {
            var controller = new NotificationController(context);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("UserId", userId)
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

        private PlantsRPetsProjetoServerContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
                .Options;

            return new PlantsRPetsProjetoServerContext(options);
        }

        [Fact]
        public async Task GetUserNotifications_ReturnsUserNotifications()
        {
            var context = GetInMemoryDbContext();
            string userId = "user123";

            context.Notification.Add(new Notification { NotificationId = 1, Message = "Test message", Type = "Info" });
            context.UserNotifications.Add(new UserNotification
            {
                UserNotificationId = 1,
                UserId = userId,
                NotificationId = 1,
                ReceivedDate = System.DateTime.UtcNow,
                isRead = false
            });
            await context.SaveChangesAsync();

            var controller = GetControllerWithContext(userId, context);

            var result = await controller.GetUserNotifications();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var notifications = Assert.IsAssignableFrom<IEnumerable<UserNotification>>(okResult.Value);
            Assert.Single(notifications);
        }

        [Fact]
        public async Task SendNotification_CreatesUserNotification()
        {
            var context = GetInMemoryDbContext();
            string userId = "user456";

            context.Notification.Add(new Notification { NotificationId = 2, Message = "Water your plant!", Type = "Reminder" });
            await context.SaveChangesAsync();

            var controller = GetControllerWithContext(userId, context);

            var result = await controller.SendNotification(2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Notification sent to user.", okResult.Value);

            var added = context.UserNotifications.FirstOrDefault(un => un.UserId == userId && un.NotificationId == 2);
            Assert.NotNull(added);
        }

        [Fact]
        public async Task MarkAsRead_UpdatesIsReadStatus()
        {
            var context = GetInMemoryDbContext();
            string userId = "user789";

            var userNotification = new UserNotification
            {
                UserNotificationId = 3,
                UserId = userId,
                NotificationId = 3,
                isRead = false,
                ReceivedDate = System.DateTime.UtcNow
            };

            context.UserNotifications.Add(userNotification);
            await context.SaveChangesAsync();

            var controller = GetControllerWithContext(userId, context);

            var result = await controller.MarkAsRead(3);

            Assert.IsType<OkResult>(result);
            Assert.True(context.UserNotifications.First(un => un.UserNotificationId == 3).isRead);
        }

        [Fact]
        public async Task UpdateEmailFrequency_ValidUpdate_ReturnsOk()
        {
            var context = GetInMemoryDbContext();
            string userId = "user987";
            context.Users.Add(new User
            {
                Nickname = "Teste",
                Id = userId,
                Email = "test@email.com",
                NotificationFrequency = User.EmailFrequency.Daily
            });
            await context.SaveChangesAsync();

            var controller = GetControllerWithContext(userId, context);
            var result = await controller.UpdateEmailFrequency((int)User.EmailFrequency.Monthly);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Monthly", ok.Value.ToString());
        }

        [Fact]
        public async Task DeleteNotification_RemovesUserNotification()
        {
            var context = GetInMemoryDbContext();
            var userNotification = new UserNotification
            {
                UserNotificationId = 4,
                UserId = "user000",
                NotificationId = 4,
                isRead = false,
                ReceivedDate = System.DateTime.UtcNow
            };

            context.UserNotifications.Add(userNotification);
            await context.SaveChangesAsync();

            var controller = GetControllerWithContext("user000", context);

            var result = await controller.DeleteNotification(4);

            Assert.IsType<OkResult>(result);
            Assert.Null(context.UserNotifications.Find(4));
        }
    }
}
