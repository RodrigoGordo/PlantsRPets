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

        //[Fact]
        //public async Task Login_ValidCredentials_ReturnsToken()
        //{
        //    // Arrange
        //    var user = new User { Email = "validuser@test.com", Nickname = "ValidUser" };
        //    var model = new UserLoginModel { Email = user.Email, Password = "CorrectPassword" };

        //    _mockUserManager.Setup(um => um.FindByEmailAsync(model.Email))
        //                    .ReturnsAsync(user);

        //    _mockUserManager.Setup(um => um.CheckPasswordAsync(user, model.Password))
        //                    .ReturnsAsync(true);

        //    _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("very_secret_key_1234567890_256bits_!"); // JWT Key mock
        //    _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("PlantsRPets");
        //    _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("PlantsRPetsUsers");

        //    // Act
        //    var result = await _controller.Login(model) as OkObjectResult;

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(200, result.StatusCode);
        //    Assert.NotNull(result.Value);

        //    var response = result.Value as dynamic;
        //    Assert.NotNull(response.token);
        //    Assert.True(response.token.ToString().Length > 10); // Verifica se o token foi gerado
        //}


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

        [Fact]
        public async Task ResetPassword_DifferentPasswords_ReturnsBadRequest()
        {
            // Arrange
            var user = new User { Email = "user@test.com" };
            var model = new ResetPasswordModel
            {
                Email = user.Email,
                Token = "valid-token",
                NewPassword = "NewPassword123"
            };

            _mockUserManager.Setup(um => um.FindByEmailAsync(model.Email))
                            .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.ResetPasswordAsync(user, model.Token, model.NewPassword))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Passwords do not match." }));

            // Act
            var result = await _controller.ResetPassword(model) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Failed to reset password.", result.Value.ToString());
        }

        [Fact]
        public async Task ResetPassword_SuccessfulReset_ReturnsOk()
        {
            // Arrange
            var user = new User { Email = "user@test.com" };
            var model = new ResetPasswordModel
            {
                Email = user.Email,
                Token = "valid-token",
                NewPassword = "NewPassword123"
            };

            _mockUserManager.Setup(um => um.FindByEmailAsync(model.Email))
                            .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.ResetPasswordAsync(user, model.Token, model.NewPassword))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.ResetPassword(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Contains("Password reset successfully", result.Value.ToString());
        }
    }
}