using Microsoft.AspNetCore.Mvc;
using Moq;
using PlantsRPetsProjeto.Server.Controllers;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace PlantsRPetsProjeto.Tests.xUnitTests.ControllerTests
{
    public class SustainabilityTipsControllerTests
    {
        private readonly SustainabilityTipsController _controller;
        private readonly PlantsRPetsProjetoServerContext _context;

        public SustainabilityTipsControllerTests()
        {
            // Create an In-Memory DbContext for testing purposes
            var options = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                            .UseInMemoryDatabase(databaseName: "InMemoryDb")
                            .Options;

            _context = new PlantsRPetsProjetoServerContext(options);
            _controller = new SustainabilityTipsController(_context);

            // Seed data into the in-memory database
            _context.SustainabilityTip.Add(new SustainabilityTip
            {
                Title = "Save Water",
                Content = "Conserve water by using water-efficient appliances.",
                Category = "Water Conservation",
                AuthorId = "author1"
            });

            _context.SaveChanges();
        }

        // 1. Test Create
        [Fact]
        public async Task CreateSustainabilityTip_ReturnsCreatedAtActionResult_WhenValidTip()
        {
            // Arrange
            var newTip = new SustainabilityTip
            {
                Title = "Save Energy",
                Content = "Use energy-efficient appliances.",
                Category = "Energy Conservation",
                AuthorId = "author2"
            };

            // Act
            var result = await _controller.CreateSustainabilityTip(newTip);

            var createdAtActionResult = Assert.IsType<ActionResult<SustainabilityTip>>(result);
            var value = Assert.IsType<SustainabilityTip>(createdAtActionResult.Value);

            // Assert the tip's properties
            Assert.Equal(newTip.Title, value.Title);
            Assert.Equal(newTip.Content, value.Content);
            Assert.Equal(newTip.Category, value.Category);
            Assert.Equal(newTip.AuthorId, value.AuthorId);
        }



        // 2. Test Get (All)
        [Fact]
        public async Task GetSustainabilityTips_ReturnsListOfTips()
        {
            // Act
            var result = await _controller.GetSustainabilityTips();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Expect OkObjectResult
            var returnedTips = Assert.IsType<List<SustainabilityTip>>(okResult.Value);
            Assert.NotEmpty(returnedTips); // Ensure list is not empty
            Assert.Equal(1, returnedTips.Count); // Since we have one seed in memory
        }


        [Fact]
        public async Task GetSustainabilityTip_ReturnsNotFound_WhenInvalidId()
        {
            // Act
            var result = await _controller.GetSustainabilityTip(999); // A non-existent ID

            // Assert
            Assert.IsType<NotFoundResult>(result); // Correctly expect NotFoundResult
        }


        [Fact]
        public async Task GetSustainabilityTip_ReturnsSustainabilityTip_WhenValidId()
        {
            // Act
            var result = await _controller.GetSustainabilityTip(1); // Valid ID

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Expect OkObjectResult
            var returnedTip = Assert.IsType<SustainabilityTip>(okResult.Value);
            Assert.Equal(1, returnedTip.Id); // Check the returned tip
        }


        // 4. Test Update
        [Fact]
        public async Task UpdateSustainabilityTip_ReturnsNoContent_WhenValidTip()
        {
            // Arrange
            var updatedTip = new SustainabilityTip
            {
                Id = 1,  // Ensure the ID matches the ID in the database (in this case, 1)
                Title = "Save Water Updated",
                Content = "Updated description for water conservation.",
                Category = "Water Conservation",
                AuthorId = "author1"
            };

            // Act
            var result = await _controller.UpdateSustainabilityTip(1, updatedTip);

            // Assert
            Assert.IsType<NoContentResult>(result);  // Expect NoContentResult

            // Verify the tip was updated in the in-memory database
            var tipInDb = await _context.SustainabilityTip.FindAsync(1);
            Assert.NotNull(tipInDb);
            Assert.Equal("Save Water Updated", tipInDb.Title);
            Assert.Equal("Updated description for water conservation.", tipInDb.Content);
        }



        [Fact]
        public async Task UpdateSustainabilityTip_ReturnsNotFound_WhenTipDoesNotExist()
        {
            // Arrange
            var updatedTip = new SustainabilityTip
            {
                Id = 999, // Non-existent ID
                Title = "Non-existent Tip",
                Content = "This tip doesn't exist in the database.",
                Category = "General",
                AuthorId = "author3"
            };

            // Act
            var result = await _controller.UpdateSustainabilityTip(999, updatedTip);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // 5. Test Delete
        [Fact]
        public async Task DeleteSustainabilityTip_ReturnsNoContent_WhenTipExists()
        {
            // Act
            var result = await _controller.DeleteSustainabilityTip(1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify that the tip was deleted
            var deletedTip = await _context.SustainabilityTip.FindAsync(1);
            Assert.Null(deletedTip); // It should be null after deletion
        }

        [Fact]
        public async Task DeleteSustainabilityTip_ReturnsNotFound_WhenTipDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteSustainabilityTip(999); // Non-existent ID

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
