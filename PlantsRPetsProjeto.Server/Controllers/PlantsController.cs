using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/plants")]
    public class PlantsController : ControllerBase
    {
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly PlantInfoService _plantInfoService;

        public PlantsController(PlantsRPetsProjetoServerContext context, PlantInfoService plantInfoService)
        {
            _context = context;
            _plantInfoService = plantInfoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlantInfo>>> GetPlants()
        {
            var plants = await _context.PlantInfo.ToListAsync();
            return Ok(plants);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PlantInfo>> GetPlant(int id)
        {
            var plant = await _context.PlantInfo.FindAsync(id);
            if (plant == null)
                return NotFound(new { message = "Plant not found." });

            return Ok(plant);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            var plant = await _context.PlantInfo.FindAsync(id);
            if (plant == null)
                return NotFound(new { message = "Plant not found." });

            _context.PlantInfo.Remove(plant);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Plant deleted successfully." });
        }

        [HttpGet("check-planting-period/{id}")]
        public async Task<IActionResult> CheckPlantingPeriod(int id)
        {
            var plant = await _context.PlantInfo.FindAsync(id);
            if (plant == null)
                return NotFound(new { message = "Plant not found." });

            Console.WriteLine($"HarvestSeason: {plant.HarvestSeason}");
            Console.WriteLine($"PruningMonths: {string.Join(",", plant.PruningMonth)}");

            var isIdeal = PlantingAdvisor.IsIdealPlantingTime(plant);

            var idealMonths = PlantingAdvisor.GetIdealPlantingMonths(plant)
                                             .Select(m => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m))
                                             .ToList();

            return Ok(new
            {
                isIdealTime = isIdeal,
                currentMonth = DateTime.UtcNow.ToString("MMMM"),
                idealMonths
            });
        }

        [HttpGet("fetch-range/{startId}/{maxId}")]
        public async Task<IActionResult> FetchAndStorePlants(int startId, int maxId)
        {
            if (startId <= 0 || maxId <= 0)
            {
                return BadRequest(new { message = "startId and maxId must be greater than zero." });
            }

            if (startId > maxId)
            {
                return BadRequest(new { message = "startId must be less than or equal to maxId." });
            }

            try
            {
                var plants = await _plantInfoService.GetPlantsAsync(startId, maxId);

                if (plants == null || !plants.Any())
                {
                    return NotFound(new { message = "No plants found for the given range." });
                }

                await SavePlantInfo(plants);
                return Ok(plants);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { message = "Error fetching data from API", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpPost("/saveInfo")]
        public async Task SavePlantInfo(List<PlantInfo> plants)
        {
            foreach (var plant in plants)
            {
                var existingPlant = await _context.PlantInfo
                    .Include(p => p.PruningCount)
                    .FirstOrDefaultAsync(p => p.PlantName.Equals(plant.PlantName));

                if (existingPlant != null)
                {
                    existingPlant.PlantName = plant.PlantName;
                    existingPlant.PlantType = plant.PlantType;
                    existingPlant.Cycle = plant.Cycle;
                    existingPlant.Watering = plant.Watering;
                    existingPlant.PruningMonth = plant.PruningMonth;
                    existingPlant.GrowthRate = plant.GrowthRate;
                    existingPlant.Sunlight = plant.Sunlight;
                    existingPlant.Edible = plant.Edible;
                    existingPlant.CareLevel = plant.CareLevel;
                    existingPlant.Flowers = plant.Flowers;
                    existingPlant.Fruits = plant.Fruits;
                    existingPlant.Leaf = plant.Leaf;
                    existingPlant.Maintenance = plant.Maintenance;
                    existingPlant.SaltTolerant = plant.SaltTolerant;
                    existingPlant.Indoor = plant.Indoor;
                    existingPlant.FloweringSeason = plant.FloweringSeason;
                    existingPlant.Description = plant.Description;
                    existingPlant.Image = plant.Image;
                    existingPlant.HarvestSeason = plant.HarvestSeason;
                    existingPlant.ScientificName = plant.ScientificName;
                    existingPlant.DroughtTolerant = plant.DroughtTolerant;
                    existingPlant.Cuisine = plant.Cuisine;
                    existingPlant.Medicinal = plant.Medicinal;

                    if (plant.PruningCount != null)
                    {
                        if (existingPlant.PruningCount == null)
                        {
                            existingPlant.PruningCount = new PruningCountInfo
                            {
                                Amount = plant.PruningCount.Amount,
                                Interval = plant.PruningCount.Interval
                            };
                        }
                        else
                        {
                            existingPlant.PruningCount.Amount = plant.PruningCount.Amount;
                            existingPlant.PruningCount.Interval = plant.PruningCount.Interval;
                        }
                    }
                    else
                    {
                        existingPlant.PruningCount = null;
                    }

                    _context.PlantInfo.Update(existingPlant);
                }
                else
                {
                    _context.PlantInfo.Add(plant);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    public class CreatePlantModel
    {
        public required string PlantName { get; set; }
        public required string Type { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Growth time must be at least 1 day.")]
        public required int GrowthTime { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Water frequency must be at least 1 day.")]
        public required int WaterFrequency { get; set; }
    }

    public class UpdatePlantModel
    {
        [Required]
        public string? PlantName { get; set; }
        public string? Type { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Growth time must be at least 1 day.")]
        public int? GrowthTime { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Water frequency must be at least 1 day.")]
        public int? WaterFrequency { get; set; }
    }

}
