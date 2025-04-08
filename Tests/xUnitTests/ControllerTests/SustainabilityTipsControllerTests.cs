using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Controllers;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;
using PlantsRPetsProjeto.Server.Data;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SustainabilityTipsControllerTests
{
    private readonly Mock<SustainabilityTipService> _mockTipService;
    private readonly Mock<PlantsRPetsProjetoServerContext> _mockContext;
    private readonly SustainabilityTipsController _controller;

    public SustainabilityTipsControllerTests()
    {
        _mockTipService = new Mock<SustainabilityTipService>();
        _mockContext = new Mock<PlantsRPetsProjetoServerContext>();
        _controller = new SustainabilityTipsController(_mockTipService.Object, _mockContext.Object);
    }

    [Fact]
    public async Task FetchAndStoreSustainabilityTips_ValidRange_ReturnsOk()
    {
        var startId = 1;
        var maxId = 10;
        var tipsList = new List<SustainabilityTipsList>
        {
            new SustainabilityTipsList { PlantInfoId = 1, PlantName = "Rose", PlantScientificName = ["Rosa rubiginosa"] }
        };

        _mockTipService.Setup(service => service.GetSustainabilityTipsAsync(startId, maxId))
                       .ReturnsAsync(tipsList);

        var result = await _controller.FetchAndStoreSustainabilityTips(startId, maxId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<SustainabilityTipsList>>(okResult.Value);
        Assert.NotEmpty(returnValue);
    }

    [Fact]
    public async Task FetchAndStoreSustainabilityTips_InvalidRange_ReturnsBadRequest()
    {
        var startId = -1;
        var maxId = 0;

        var result = await _controller.FetchAndStoreSustainabilityTips(startId, maxId);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid ID range", badRequestResult.Value);
    }

    [Fact]
    public async Task FetchAndStoreSustainabilityTips_NoTipsFound_ReturnsNotFound()
    {
        var startId = 1;
        var maxId = 10;

        _mockTipService.Setup(service => service.GetSustainabilityTipsAsync(startId, maxId))
                       .ReturnsAsync(new List<SustainabilityTipsList>());

        var result = await _controller.FetchAndStoreSustainabilityTips(startId, maxId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No tips found in this range", notFoundResult.Value);
    }
}
