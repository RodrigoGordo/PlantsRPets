using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PlantsRPetsProjeto.Server.Controllers;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using Xunit;

namespace PlantsRPetsProjeto.Tests.xUnitTests.Controllers
{
    public class ProfilesControllerTests : IDisposable
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly DbContextOptions<PlantsRPetsProjetoServerContext> _dbContextOptions;
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly ProfilesController _controller;

        public ProfilesControllerTests()
        {
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(
                store.Object, null, null, null, null, null, null, null, null);

            _dbContextOptions = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PlantsRPetsProjetoServerContext(_dbContextOptions);
            _context.Database.EnsureCreated();

            _controller = new ProfilesController(_context, _mockUserManager.Object);

            SetupAuthenticatedUser();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SetupAuthenticatedUser()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[] { new Claim("UserId", "test-user-id") },
                "TestAuthentication"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        private async Task CreateTestUser(string nickname = "TestNickname", string bio = null)
        {
            var testUser = new User
            {
                Id = "test-user-id",
                UserName = "testuser",
                Nickname = nickname,
                Email = "test@example.com"
            };

            var testProfile = new Profile
            {
                UserId = "test-user-id",
                Bio = bio
            };

            await _context.Users.AddAsync(testUser);
            await _context.Profile.AddAsync(testProfile);
            await _context.SaveChangesAsync();
        }

        //[Fact]
        //public async Task UpdateProfile_UnauthenticatedUser_ReturnsUnauthorized()
        //{
        //    // Arrange
        //    var unauthenticatedController = new ProfilesController(_context, _mockUserManager.Object);

        //    // Act
        //    var result = await unauthenticatedController.UpdateProfile(new ProfilesController.UpdateProfileModel());

        //    // Assert
        //    Assert.IsType<>(result);
        //}

        [Fact]
        public async Task UpdateProfile_UserNotFound_ReturnsNotFound()
        {
           var model = new ProfilesController.UpdateProfileModel { Nickname = "NewNickname" };

            var result = await _controller.UpdateProfile(model);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProfile_ValidNickname_UpdatesSuccessfully()
        {
            await CreateTestUser("OldNickname", "Old Bio");
            var model = new ProfilesController.UpdateProfileModel { Nickname = "NewNickname" };

            var result = await _controller.UpdateProfile(model);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedProfile = Assert.IsType<Profile>(okResult.Value);

            var userInDb = await _context.Users.FindAsync("test-user-id");
            Assert.Equal("NewNickname", userInDb.Nickname);
            Assert.Equal("Old Bio", updatedProfile.Bio);
        }

        [Fact]
        public async Task UpdateProfile_ValidBio_UpdatesSuccessfully()
        {
            await CreateTestUser(bio: "Old Bio");
            var model = new ProfilesController.UpdateProfileModel { Bio = "New Bio" };

            var result = await _controller.UpdateProfile(model);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedProfile = Assert.IsType<Profile>(okResult.Value);
            Assert.Equal("New Bio", updatedProfile.Bio);
            Assert.Equal("TestNickname", (await _context.Users.FindAsync("test-user-id")).Nickname);
        }

        [Fact]
        public async Task UpdateProfile_FavoritePets_UpdatesSuccessfully()
        {
            await CreateTestUser();
            var favoritePets = new List<int> { 1, 3, 5 };
            var model = new ProfilesController.UpdateProfileModel { FavoritePets = favoritePets };

            var result = await _controller.UpdateProfile(model);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedProfile = Assert.IsType<Profile>(okResult.Value);
            Assert.Equal(favoritePets, updatedProfile.FavoritePets);
        }

        [Fact]
        public async Task UpdateProfile_HighlightedPlantations_UpdatesSuccessfully()
        {
            await CreateTestUser();
            var highlightedPlantations = new List<int> { 2, 4, 6 };
            var model = new ProfilesController.UpdateProfileModel { HighlightedPlantations = highlightedPlantations };

            var result = await _controller.UpdateProfile(model);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedProfile = Assert.IsType<Profile>(okResult.Value);
            Assert.Equal(highlightedPlantations, updatedProfile.HighlightedPlantations);
        }

        [Fact]
        public async Task CreateUser_WithoutNickname_ThrowsException()
        {
            var invalidUser = new User
            {
                Id = "invalid-user",
                UserName = "invaliduser",
                Email = "invalid@example.com"
            };

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _context.Users.AddAsync(invalidUser);
                await _context.SaveChangesAsync();
            });
        }

        [Fact]
        public async Task SaveProfilePicture_InvalidFile_ThrowsException()
        {
            var controller = new ProfilesController(_context, _mockUserManager.Object);
            IFormFile nullFile = null;

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await controller.SaveProfilePicture(nullFile);
            });
        }
    }
}