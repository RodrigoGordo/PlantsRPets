using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PlantsRPetsProjeto.Server.Controllers;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlantsRPetsProjeto.Tests.xUnitTests.ControllerTests
{
    public class TutorialsControllerTests
    {
        private readonly TutorialsController _controller;
        private readonly PlantsRPetsProjetoServerContext _context;

        public TutorialsControllerTests()
        {
            // Set up an in-memory database for testing
            var options = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                            .UseInMemoryDatabase("InMemoryDbTutorials")
                            .Options;

            _context = new PlantsRPetsProjetoServerContext(options);
            _controller = new TutorialsController(_context);

            // Seed the in-memory database with a tutorial
            _context.Tutorial.Add(new Tutorial
            {
                Title = "How to Water Plants",
                Content = "Instructions on watering plants.",
                AuthorId = "author1"
            });

            _context.SaveChanges();
        }

        // 1. Test Get All Tutorials
        [Fact]
        public async Task GetTutorials_ReturnsOkResult_WithTutorialList()
        {
            // Act
            var result = await _controller.GetTutorials();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tutorials = Assert.IsType<List<Tutorial>>(okResult.Value);
            Assert.Single(tutorials); // Assert that one tutorial is in the database
        }

        // 2. Test Get Single Tutorial (Valid ID)
        [Fact]
        public async Task GetTutorial_ReturnsOkResult_WhenTutorialExists()
        {
            // Act
            var result = await _controller.GetTutorial(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tutorial = Assert.IsType<Tutorial>(okResult.Value);
            Assert.Equal(1, tutorial.Id); // Validate that we get the tutorial with ID 1
        }

        // 3. Test Get Single Tutorial (Invalid ID)
        [Fact]
        public async Task GetTutorial_ReturnsNotFound_WhenTutorialDoesNotExist()
        {
            // Act
            var result = await _controller.GetTutorial(999); // ID 999 doesn't exist

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // 4. Test Create Tutorial (Valid Data)
        [Fact]
        public async Task CreateTutorial_ReturnsCreatedAtActionResult_WhenValidTutorial()
        {
            var newTutorial = new Tutorial
            {
                Title = "How to Plant Trees",
                Content = "Step-by-step guide on planting trees.",
                AuthorId = "author2"
            };

            // Act
            var result = await _controller.CreateTutorial(newTutorial);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var value = Assert.IsType<Tutorial>(createdAtActionResult.Value);

            // Check if RouteValues is not null
            Assert.NotNull(createdAtActionResult.RouteValues);
            Assert.Equal(newTutorial.Title, value.Title);
            Assert.Equal(newTutorial.Content, value.Content);
            Assert.Equal(newTutorial.AuthorId, value.AuthorId);

        }


        // 5. Test Create Tutorial (Invalid Data)
        [Fact]
        public async Task CreateTutorial_ReturnsBadRequest_WhenInvalidData()
        {
            var invalidTutorial = new Tutorial
            {
                Title = "", // Invalid title
                Content = "Some content",
                AuthorId = "author2"
            };

            // Act
            var result = await _controller.CreateTutorial(invalidTutorial);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // 6. Test Update Tutorial (Valid Data)
        [Fact]
        public async Task UpdateTutorial_ReturnsNoContent_WhenTutorialUpdated()
        {
            var updatedTutorial = new Tutorial
            {
                Title = "How to Water Plants (Updated)",
                Content = "Updated content on watering plants.",
                AuthorId = "author1"
            };

            // Act
            var result = await _controller.UpdateTutorial(1, updatedTutorial);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // 7. Test Update Tutorial (ID Mismatch)
        [Fact]
        public async Task UpdateTutorial_ReturnsBadRequest_WhenIdMismatch()
        {
            var updatedTutorial = new Tutorial
            {
                Title = "Updated tutorial",
                Content = "Updated content",
                AuthorId = "author2"
            };

            // Act
            var result = await _controller.UpdateTutorial(999, updatedTutorial);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // 8. Test Delete Tutorial (Valid ID)
        [Fact]
        public async Task DeleteTutorial_ReturnsNoContent_WhenTutorialDeleted()
        {
            // Act
            var result = await _controller.DeleteTutorial(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // 9. Test Delete Tutorial (Invalid ID)
        [Fact]
        public async Task DeleteTutorial_ReturnsNotFound_WhenTutorialDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteTutorial(999); // ID 999 doesn't exist

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // 10. Test Delete Tutorial (Invalid ID)
        [Fact]
        public async Task DeleteTutorial_ReturnsBadRequest_WhenInvalidId()
        {
            // Act
            var result = await _controller.DeleteTutorial(0); // Invalid ID

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
