using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Controllers;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace PlantsRPetsProjeto.Tests.xUnitTests.ControllerTests
{
    public class PlantationControllerTests
    {
        private readonly PlantationsController _controller;
        private readonly PlantsRPetsProjetoServerContext _context;

        public PlantationControllerTests()
        {
            var options = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                            .UseInMemoryDatabase("InMemoryDbPlantations")
                            .Options;

            _context = new PlantsRPetsProjetoServerContext(options);
            _controller = new PlantationsController(_context, new PlantInfoService(new HttpClient(), _context));

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", "1")
            }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        private void SeedDatabase()
        {
            _context.Plantation.Add(new Plantation
            {
                PlantationId = 1,
                OwnerId = "1",
                PlantationName = "My First Plantation",
                PlantTypeId = 1,
                PlantingDate = DateTime.UtcNow,
                ExperiencePoints = 0,
                Level = 1
            });

            _context.PlantType.Add(new PlantType
            {
                PlantTypeId = 1,
                PlantTypeName = "Tomato"
            });

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetUserPlantations_ReturnsListOfPlantations_WhenPlantationsExist()
        {
            SeedDatabase();

            var result = await _controller.GetUserPlantations();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var plantations = okResult.Value as IEnumerable<object>;
            Assert.NotNull(plantations);
            Assert.Single(plantations);

            var firstPlantation = plantations.First();
            var plantationId = firstPlantation.GetType().GetProperty("PlantationId")?.GetValue(firstPlantation);
            var plantationName = firstPlantation.GetType().GetProperty("PlantationName")?.GetValue(firstPlantation);
            var plantTypeName = firstPlantation.GetType().GetProperty("PlantTypeName")?.GetValue(firstPlantation);

            Assert.Equal(1, plantationId);
            Assert.Equal("My First Plantation", plantationName);
            Assert.Equal("Tomato", plantTypeName);
        }

        [Fact]
        public async Task GetPlantation_ReturnsPlantation_WhenIdIsValid()
        {
            SeedDatabase();

            var result = await _controller.GetPlantation(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var plantation = okResult.Value;
            Assert.NotNull(plantation);
        }

        [Fact]
        public async Task GetPlantation_ReturnsNotFound_WhenIdIsInvalid()
        {
            SeedDatabase();

            var result = await _controller.GetPlantation(999);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreatePlantation_ReturnsCreatedAtActionResult_WhenDataIsValid()
        {
            var newPlantation = new CreatePlantationModel
            {
                PlantationName = "New Plantation",
                PlantTypeId = 1
            };

            var result = await _controller.CreatePlantation(newPlantation);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var plantation = Assert.IsType<Plantation>(createdAtActionResult.Value);
            Assert.Equal("New Plantation", plantation.PlantationName);
        }

        [Fact]
        public async Task CreatePlantation_ReturnsBadRequest_WhenPlantationNameIsEmpty()
        {
            var invalidPlantation = new CreatePlantationModel
            {
                PlantationName = "",
                PlantTypeId = 1
            };

            var result = await _controller.CreatePlantation(invalidPlantation);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdatePlantation_ReturnsNoContent_WhenDataIsValid()
        {
            SeedDatabase();
            var updatedPlantation = new UpdatePlantationModel
            {
                PlantationName = "Updated Plantation Name"
            };

            var result = await _controller.UpdatePlantation(1, updatedPlantation);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdatePlantation_ReturnsNotFound_WhenIdDoesNotExist()
        {
            SeedDatabase();
            var updatedPlantation = new UpdatePlantationModel
            {
                PlantationName = "Updated Plantation Name"
            };

            var result = await _controller.UpdatePlantation(999, updatedPlantation);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeletePlantation_ReturnsNoContent_WhenIdIsValid()
        {
            SeedDatabase();

            var result = await _controller.DeletePlantation(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeletePlantation_ReturnsNotFound_WhenIdDoesNotExist()
        {
            SeedDatabase();

            var result = await _controller.DeletePlantation(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeletePlantation_ReturnsBadRequest_WhenIdIsZero()
        {
            SeedDatabase();

            var result = await _controller.DeletePlantation(0);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}