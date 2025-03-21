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
using PlantsRPetsProjeto.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Linq.Expressions;

namespace PlantsRPetsProjeto.Tests.xUnitTests.ControllerTests
{
    public class UserAccountsControllerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<DbSet<Profile>> _mockProfileDbSet;
        private readonly PlantsRPetsProjetoServerContext _mockContext;
        private readonly UserAccountsController _controller;

        public UserAccountsControllerTests()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);

            _mockEmailService = new Mock<IEmailService>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockProfileDbSet = new Mock<DbSet<Profile>>();

            var options = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase" + Guid.NewGuid().ToString())
                .Options;

            _mockContext = new PlantsRPetsProjetoServerContext(options);

            _controller = new UserAccountsController(
                _mockUserManager.Object,
                _mockConfiguration.Object,
                _mockEmailService.Object,
                _mockContext
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
            var user = new User { Id = "user123", UserName = model.Email, Email = model.Email, Nickname = model.Nickname };

            _mockUserManager.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync((User)null);
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), model.Password))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<User, string>((u, p) => { u.Id = user.Id; }); 

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

            _mockConfiguration.Setup(c => c["Frontend:BaseUrl"]).Returns("http://localhost:3000");

            var model = new ForgotPasswordModel { Email = user.Email };
            var result = await _controller.ForgotPassword(model);

            _mockEmailService.Verify(e => e.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ResetPassword_DifferentPasswords_ReturnsBadRequest()
        {
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

            var result = await _controller.ResetPassword(model) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("Failed to reset password.", result.Value.ToString());
        }

        [Fact]
        public async Task ResetPassword_SuccessfulReset_ReturnsOk()
        {
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

            var result = await _controller.ResetPassword(model) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Contains("Password reset successfully", result.Value.ToString());
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            var user = new User { Id = "test-user-id", Email = "validuser@test.com", Nickname = "ValidUser" };
            var model = new UserLoginModel { Email = user.Email, Password = "CorrectPassword" };

            _mockUserManager.Setup(um => um.FindByEmailAsync(model.Email))
                            .ReturnsAsync(user);

            _mockUserManager.Setup(um => um.CheckPasswordAsync(user, model.Password))
                            .ReturnsAsync(true);

            _mockUserManager.Setup(um => um.GetRolesAsync(user))
                            .ReturnsAsync(new List<string> { "User" });

            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("very_secret_key_1234567890_256bits_!");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("PlantsRPets");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("PlantsRPetsUsers");

            var result = await _controller.Login(model) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            var resultValue = result.Value;
            var tokenProperty = resultValue.GetType().GetProperty("token");
            var token = tokenProperty.GetValue(resultValue, null) as string;

            Assert.NotNull(token);
            Assert.True(token.Length > 10);
        }



    }
}