using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using PlantsRPetsProjeto.Server.Controllers;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;
using System.Collections.Generic;

namespace PlantsRPetsProjeto.Tests.xUnitTests.ControllerTests
{
    public class UserAccountsControllerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly UserAccountsController _controller;

        public UserAccountsControllerTests()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);

            _mockEmailService = new Mock<IEmailService>();
            _mockConfiguration = new Mock<IConfiguration>();

            _controller = new UserAccountsController(
                _mockUserManager.Object,
                _mockConfiguration.Object,
                _mockEmailService.Object
            );
        }

        [Fact]
        public async Task Register_UserAlreadyExists_ReturnsBadRequest()
        {
            var model = new UserRegistrationModel { Email = "existinguser@test.com", Nickname = "Nick", Password = "Password123" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync(new User());

            var result = await _controller.Register(model);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_NewUser_ReturnsOk()
        {
            var model = new UserRegistrationModel { Email = "newuser@test.com", Nickname = "Nick", Password = "Password123" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync((User)null);
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), model.Password)).ReturnsAsync(IdentityResult.Success);

            var result = await _controller.Register(model);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            var model = new UserLoginModel { Email = "invalid@test.com", Password = "wrongpassword" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync((User)null);

            var result = await _controller.Login(model);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ForgotPassword_UserNotFound_ReturnsBadRequest()
        {
            var model = new ForgotPasswordModel { Email = "nonexistent@test.com" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync((User)null);

            var result = await _controller.ForgotPassword(model);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ForgotPassword_ValidUser_SendsEmail()
        {
            var user = new User { Email = "valid@test.com", Nickname = "ValidUser" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("mocked-token");

            var model = new ForgotPasswordModel { Email = user.Email };
            var result = await _controller.ForgotPassword(model);

            _mockEmailService.Verify(e => e.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

    }
}