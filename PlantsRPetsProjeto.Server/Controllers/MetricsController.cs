using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/metrics")]
    public class MetricsController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly MetricsService _metricsService;

        public MetricsController(PlantsRPetsProjetoServerContext context, MetricsService metricsService)
        {
            _context = context;
            _metricsService = metricsService;
        }

        [HttpGet("metrics/activity-counts")]
        public async Task<IActionResult> GetActivityCounts([FromQuery] string timeFrame = "week")
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var counts = await _metricsService.GetActivityCountsAsync(userId, timeFrame);
            return Ok(counts);
        }

        [HttpGet("metrics/activity-by-date")]
        public async Task<IActionResult> GetActivityByDate([FromQuery] string eventType, [FromQuery] string timeFrame = "week")
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var data = await _metricsService.GetActivityByDateAsync(userId, eventType, timeFrame);
            return Ok(data);
        }

        [HttpGet("metrics/plant-type-distribution")]
        public async Task<IActionResult> GetPlantTypeDistribution()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var data = await _metricsService.GetPlantTypeDistributionAsync(userId);
            return Ok(data);

        }
    }

}