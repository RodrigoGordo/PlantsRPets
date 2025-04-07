using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PlantsRPetsProjeto.Server.Controllers;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;
using Xunit;

namespace PlantsRPetsProjeto.Tests.xUnitTests.Controllers
{
    public class PlantsControllerTests : IDisposable
    {
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly Mock<PlantInfoService> _mockPlantInfoService;
        private readonly PlantsController _controller;

        public PlantsControllerTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PlantsRPetsProjetoServerContext(options);
            _mockPlantInfoService = new Mock<PlantInfoService>(MockBehavior.Strict, new HttpClient());

            // Seed test data
            SeedTestData();

            _controller = new PlantsController(_context, _mockPlantInfoService.Object);

            // Mock user context
            SetupAuthenticatedUser();
        }

        private void SeedTestData()
        {
            var plants = new List<PlantInfo>
            {
                new PlantInfo
                {
                    PlantInfoId = 1,
                    PlantName = "Rose",
                    Watering = "Frequent",
                    PruningMonth = new List<string> { "Janeiro", "Fevereiro", "Dezembro" },
                    HarvestSeason = "Summer",
                    GrowthRate = "High",
                    Sunlight = new List<string> { "Full Sun" },
                    Edible = "No",
                    CareLevel = "Medium",
                    Flowers = "Yes",
                    Fruits = "No",
                    Leaf = true,
                    Maintenance = "Medium",
                    SaltTolerant = "No",
                    Indoor = false,
                    FloweringSeason = "Spring",
                    Description = "A popular ornamental plant.",
                    Image = "rose.jpg",
                    ScientificName = new List<string> { "Rosa" },
                    DroughtTolerant = false,
                    Cuisine = false,
                    Medicinal = false
                },
                new PlantInfo
                {
                    PlantInfoId = 2,
                    PlantName = "Tulip",
                    Watering = "Moderate",
                    PruningMonth = new List<string> { "Março", "Abril" },
                    HarvestSeason = "Spring",
                    GrowthRate = "Medium",
                    Sunlight = new List<string> { "Partial Shade" },
                    Edible = "No",
                    CareLevel = "Easy",
                    Flowers = "Yes",
                    Fruits = "No",
                    Leaf = true,
                    Maintenance = "Low",
                    SaltTolerant = "No",
                    Indoor = true,
                    FloweringSeason = "Spring",
                    Description = "A bright flowering plant.",
                    Image = "tulip.jpg",
                    ScientificName = new List<string> { "Tulipa" },
                    DroughtTolerant = false,
                    Cuisine = false,
                    Medicinal = false
                }
            };

            _context.PlantInfo.AddRange(plants);
            _context.SaveChanges();
        }

        private void SetupAuthenticatedUser()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "User")
            }, "TestAuthentication"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetPlants_ReturnsAllPlants()
        {
            // Act
            var result = await _controller.GetPlants();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var plants = Assert.IsType<List<PlantInfo>>(okResult.Value);
            Assert.Equal(2, plants.Count);
        }

        [Fact]
        public async Task GetPlant_ExistingId_ReturnsPlant()
        {
            // Act
            var result = await _controller.GetPlant(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var plant = Assert.IsType<PlantInfo>(okResult.Value);
            Assert.Equal("Rose", plant.PlantName);
        }

        [Fact]
        public async Task GetPlant_NonExistingId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetPlant(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeletePlant_AdminUser_DeletesPlant()
        {
            // Arrange - Add admin role
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }, "TestAuthentication"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await _controller.DeletePlant(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Null(await _context.PlantInfo.FindAsync(1));
        }

        [Fact]
        public async Task DeletePlant_NonAdminUser_ReturnsForbidden()
        {
            // Act
            var result = await _controller.DeletePlant(1);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeletePlant_NonExistingId_ReturnsNotFound()
        {
            // Arrange - Add admin role
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }, "TestAuthentication"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await _controller.DeletePlant(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CheckPlantingPeriod_ExistingId_ReturnsPlantingInfo()
        {
            // Arrange
            var currentMonth = DateTime.UtcNow.Month;

            // Act
            var result = await _controller.CheckPlantingPeriod(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as dynamic;

            Assert.NotNull(response);
            Assert.NotNull(response.isIdealTime);
            Assert.NotNull(response.currentMonth);
            Assert.NotNull(response.idealMonths);
        }

        [Fact]
        public async Task CheckPlantingPeriod_NonExistingId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.CheckPlantingPeriod(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task SavePlantInfo_NewPlants_AddsToDatabase()
        {
            // Arrange
            var newPlants = new List<PlantInfo>
            {
                new PlantInfo { PlantInfoId = 3, PlantName = "Orchid" },
                new PlantInfo { PlantInfoId = 4, PlantName = "Cactus" }
            };

            // Act
            await _controller.SavePlantInfo(newPlants);

            // Assert
            Assert.Equal(4, _context.PlantInfo.Count());
            Assert.NotNull(_context.PlantInfo.FirstOrDefault(p => p.PlantName == "Orchid"));
        }

        [Fact]
        public async Task SavePlantInfo_ExistingPlants_UpdatesDatabase()
        {
            // Arrange
            var updatedPlants = new List<PlantInfo>
            {
                new PlantInfo { PlantInfoId = 1, PlantName = "Updated Rose", Watering = "Rare" }
            };

            // Act
            await _controller.SavePlantInfo(updatedPlants);

            // Assert
            var updatedPlant = await _context.PlantInfo.FindAsync(1);
            Assert.Equal("Updated Rose", updatedPlant.PlantName);
            Assert.Equal("Rare", updatedPlant.Watering);
        }
    }
}
