//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Moq;
//using PlantsRPetsProjeto.Server.Controllers;
//using PlantsRPetsProjeto.Server.Data;
//using PlantsRPetsProjeto.Server.Models;
//using PlantsRPetsProjeto.Server.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Xunit;

//namespace PlantsRPetsProjeto.Tests.xUnitTests.ControllerTests
//{
//    public class SustainabilityTipsControllerTests
//    {
//        private readonly SustainabilityTipsController _controller;
//        private readonly PlantsRPetsProjetoServerContext _context;
//        private readonly Mock<SustainabilityTipService> _mockTipService;

//        public SustainabilityTipsControllerTests()
//        {
//            // Setup in-memory database
//            var options = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
//                .UseInMemoryDatabase(databaseName: "PlantsRPetsProjetoServerContext-Tips-Tests")
//                .Options;

//            _context = new PlantsRPetsProjetoServerContext(options);

//            // Reset database for each test
//            _context.Database.EnsureDeleted();
//            _context.Database.EnsureCreated();

//            // Create a mock of the SustainabilityTipService
//            _mockTipService = new Mock<SustainabilityTipService>(
//                Mock.Of<HttpClient>(),
//                MockConfiguration());

//            // Create controller with dependencies
//            _controller = new SustainabilityTipsController(_mockTipService.Object, _context);

//            // Seed database with test data
//            SeedDatabase();
//        }

//        private IConfiguration MockConfiguration()
//        {
//            var mockConfig = new Mock<IConfiguration>();
//            mockConfig.Setup(c => c["ApiSettings:PerenualApiKey"]).Returns("mock-api-key");
//            return mockConfig.Object;
//        }

//        private void SeedDatabase()
//        {
//            // Add a plant with tips
//            var tipsList = new SustainabilityTipsList
//            {
//                SustainabilityTipsListId = 1,
//                PlantInfoId = 100,
//                PlantName = "Snake Plant",
//                PlantScientificName = new List<string> { "Sansevieria trifasciata" },
//                SustainabilityTip = new List<SustainabilityTip>
//                {
//                    new SustainabilityTip
//                    {
//                        SustainabilityTipId = 1,
//                        Type = "Watering",
//                        Description = "Water sparingly, only when soil is completely dry.",
//                        SustainabilityTipsListId = 1
//                    },
//                    new SustainabilityTip
//                    {
//                        SustainabilityTipId = 2,
//                        Type = "Light",
//                        Description = "Tolerates low light but thrives in indirect bright light.",
//                        SustainabilityTipsListId = 1
//                    }
//                }
//            };

//            _context.SustainabilityTipsList.Add(tipsList);
//            _context.SaveChanges();
//        }

//        [Fact]
//        public async Task FetchAndStoreSustainabilityTips_ReturnsBadRequest_WhenInvalidIdRange()
//        {
//            // Arrange - Invalid ranges
//            var testCases = new[]
//            {
//                (startId: 0, maxId: 5),
//                (startId: -1, maxId: 5),
//                (startId: 5, maxId: 0),
//                (startId: 5, maxId: -1),
//                (startId: 10, maxId: 5)
//            };

//            foreach (var (startId, maxId) in testCases)
//            {
//                // Act
//                var result = await _controller.FetchAndStoreSustainabilityTips(startId, maxId);

//                // Assert
//                Assert.IsType<BadRequestObjectResult>(result);
//            }
//        }

//        [Fact]
//        public async Task FetchAndStoreSustainabilityTips_ReturnsNotFound_WhenNoTipsFound()
//        {
//            // Arrange
//            _mockTipService.Setup(s => s.GetSustainabilityTipsAsync(It.IsAny<int>(), It.IsAny<int>()))
//                .ReturnsAsync(new List<SustainabilityTipsList>());

//            // Act
//            var result = await _controller.FetchAndStoreSustainabilityTips(1, 5);

//            // Assert
//            Assert.IsType<NotFoundObjectResult>(result);
//        }

//        [Fact]
//        public async Task FetchAndStoreSustainabilityTips_ReturnsOk_WhenTipsFound()
//        {
//            // Arrange
//            var mockTipsLists = new List<SustainabilityTipsList>
//            {
//                new SustainabilityTipsList
//                {
//                    PlantInfoId = 200,
//                    PlantName = "Aloe Vera",
//                    PlantScientificName = new List<string> { "Aloe vera" },
//                    SustainabilityTip = new List<SustainabilityTip>
//                    {
//                        new SustainabilityTip
//                        {
//                            Type = "Watering",
//                            Description = "Let soil dry completely between waterings."
//                        }
//                    }
//                }
//            };

//            _mockTipService.Setup(s => s.GetSustainabilityTipsAsync(1, 5))
//                .ReturnsAsync(mockTipsLists);

//            // Act
//            var result = await _controller.FetchAndStoreSustainabilityTips(1, 5);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var returnedTips = Assert.IsAssignableFrom<List<SustainabilityTipsList>>(okResult.Value);
//            Assert.Single(returnedTips);
//            Assert.Equal("Aloe Vera", returnedTips[0].PlantName);
//        }

//        [Fact]
//        public async Task FetchAndStoreSustainabilityTips_Returns502_WhenHttpRequestException()
//        {
//            // Arrange
//            _mockTipService.Setup(s => s.GetSustainabilityTipsAsync(It.IsAny<int>(), It.IsAny<int>()))
//                .ThrowsAsync(new HttpRequestException("API unavailable"));

//            // Act
//            var result = await _controller.FetchAndStoreSustainabilityTips(1, 5);

//            // Assert
//            var statusCodeResult = Assert.IsType<ObjectResult>(result);
//            Assert.Equal(502, statusCodeResult.StatusCode);
//            Assert.Contains("API Error", statusCodeResult.Value.ToString());
//        }

//        [Fact]
//        public async Task FetchAndStoreSustainabilityTips_Returns500_WhenGenericException()
//        {
//            // Arrange
//            _mockTipService.Setup(s => s.GetSustainabilityTipsAsync(It.IsAny<int>(), It.IsAny<int>()))
//                .ThrowsAsync(new Exception("Something went wrong"));

//            // Act
//            var result = await _controller.FetchAndStoreSustainabilityTips(1, 5);

//            // Assert
//            var statusCodeResult = Assert.IsType<ObjectResult>(result);
//            Assert.Equal(500, statusCodeResult.StatusCode);
//            Assert.Contains("Internal Error", statusCodeResult.Value.ToString());
//        }

//        [Fact]
//        public async Task SaveSustainabilityTips_AddsNewTipsLists_WhenNotExists()
//        {
//            // Arrange
//            var newTipsList = new SustainabilityTipsList
//            {
//                PlantInfoId = 300,
//                PlantName = "Peace Lily",
//                PlantScientificName = new List<string> { "Spathiphyllum" },
//                SustainabilityTip = new List<SustainabilityTip>
//                {
//                    new SustainabilityTip
//                    {
//                        Type = "Watering",
//                        Description = "Keep soil moist but not soggy."
//                    }
//                }
//            };

//            // Act
//            var result = await _controller.SaveSustainabilityTips(new List<SustainabilityTipsList> { newTipsList });

//            // Assert
//            Assert.IsType<OkResult>(result);

//            // Verify the database has the new entry
//            var savedTipsList = await _context.SustainabilityTipsList
//                .Include(l => l.SustainabilityTip)
//                .FirstOrDefaultAsync(l => l.PlantInfoId == 300);

//            Assert.NotNull(savedTipsList);
//            Assert.Equal("Peace Lily", savedTipsList.PlantName);
//            Assert.Single(savedTipsList.SustainabilityTip);
//        }

//        [Fact]
//        public async Task SaveSustainabilityTips_UpdatesExistingTipsList_WhenExists()
//        {
//            // Arrange
//            var updatedTipsList = new SustainabilityTipsList
//            {
//                PlantInfoId = 100, // This already exists in our seeded data
//                PlantName = "Updated Snake Plant",
//                PlantScientificName = new List<string> { "Updated Sansevieria trifasciata" },
//                SustainabilityTip = new List<SustainabilityTip>
//                {
//                    new SustainabilityTip
//                    {
//                        Type = "New Tip Type",
//                        Description = "New description"
//                    }
//                }
//            };

//            // Act
//            var result = await _controller.SaveSustainabilityTips(new List<SustainabilityTipsList> { updatedTipsList });

//            // Assert
//            Assert.IsType<OkResult>(result);

//            // Verify the database has updated the entry
//            var savedTipsList = await _context.SustainabilityTipsList
//                .Include(l => l.SustainabilityTip)
//                .FirstOrDefaultAsync(l => l.PlantInfoId == 100);

//            Assert.NotNull(savedTipsList);
//            Assert.Equal("Updated Snake Plant", savedTipsList.PlantName);
//            Assert.Single(savedTipsList.SustainabilityTip);
//            Assert.Equal("New Tip Type", savedTipsList.SustainabilityTip.First().Type);
//        }

//        [Fact]
//        public async Task GetAllSustainabilityTips_ReturnsAllTipsLists()
//        {
//            // Act
//            var result = await _controller.GetAllSustainabilityTips();

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<IEnumerable<SustainabilityTipsList>>>(result);
//            var tipsLists = Assert.IsAssignableFrom<List<SustainabilityTipsList>>(actionResult.Value);

//            Assert.Single(tipsLists);
//            Assert.Equal("Snake Plant", tipsLists[0].PlantName);
//            Assert.Equal(2, tipsLists[0].SustainabilityTip.Count);
//        }

//        [Fact]
//        public async Task GetTipsByPlant_ReturnsNotFound_WhenPlantHasNoTips()
//        {
//            // Act
//            var result = await _controller.GetTipsByPlant(999); // Non-existent plant ID

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<IEnumerable<TipDto>>>(result);
//            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
//        }

//        [Fact]
//        public async Task GetTipsByPlant_ReturnsTips_WhenPlantExists()
//        {
//            // Act
//            var result = await _controller.GetTipsByPlant(100); // This exists in our seeded data

//            // Assert
//            var actionResult = Assert.IsType<ActionResult<IEnumerable<TipDto>>>(result);
//            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
//            var tips = Assert.IsAssignableFrom<List<TipDto>>(okResult.Value);

//            Assert.Equal(2, tips.Count);
//            Assert.Contains(tips, t => t.TipType == "Watering");
//            Assert.Contains(tips, t => t.TipType == "Light");
//            Assert.All(tips, t => Assert.Equal(100, t.PlantInfoId));
//        }
//    }
//}
