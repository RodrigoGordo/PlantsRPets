using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
        {
            var plants = await _context.Plant.ToListAsync();
            return Ok(plants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(int id)
        {
            var plant = await _context.Plant.FindAsync(id);
            if (plant == null)
                return NotFound(new { message = "Plant not found." });

            return Ok(plant);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Plant>> CreatePlant([FromBody] CreatePlantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plant = new Plant
            {
                PlantName = model.PlantName,
                Type = model.Type,
                GrowthTime = model.GrowthTime,
                WaterFrequency = model.WaterFrequency
            };

            _context.Plant.Add(plant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlant), new { id = plant.PlantId }, plant);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePlant(int id, [FromBody] UpdatePlantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingPlant = await _context.Plant.FindAsync(id);
            if (existingPlant == null)
                return NotFound(new { message = "Plant not found." });

            if (model.PlantName != null)
                existingPlant.PlantName = model.PlantName;

            if (model.Type.HasValue)
                existingPlant.Type = model.Type.Value;

            if (model.GrowthTime.HasValue)
                existingPlant.GrowthTime = model.GrowthTime.Value;

            if (model.WaterFrequency.HasValue)
                existingPlant.WaterFrequency = model.WaterFrequency.Value;

            _context.Entry(existingPlant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Conflict updating plant, please try again." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            var plant = await _context.Plant.FindAsync(id);
            if (plant == null)
                return NotFound(new { message = "Plant not found." });

            _context.Plant.Remove(plant);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Plant deleted successfully." });
        }

        [HttpGet("/plants/plants")]
        public async Task<IActionResult> FetchAndStorePlant()
        {
            try
            {
                var plants = await _plantInfoService.GetPlantsAsync();
                await SavePlantInfo(plants);
                return Ok(plants);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("/bombo")]
        public async Task SavePlantInfo(List<PlantInfo> plants)
        {
            foreach (var plant in plants)
            {
                var existingPlant = await _context.PlantInfo.FindAsync(plant.PlantInfoId);
                if (existingPlant != null)
                {
                    _context.PlantInfo.Remove(existingPlant);
                }
                _context.PlantInfo.Add(plant);
            }

            await _context.SaveChangesAsync();
        }
    }

    public class CreatePlantModel
    {
        public required string PlantName { get; set; }
        public required PlantType Type { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Growth time must be at least 1 day.")]
        public required int GrowthTime { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Water frequency must be at least 1 day.")]
        public required int WaterFrequency { get; set; }
    }

    public class UpdatePlantModel
    {
        [Required]
        public string? PlantName { get; set; }
        public PlantType? Type { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Growth time must be at least 1 day.")]
        public int? GrowthTime { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Water frequency must be at least 1 day.")]
        public int? WaterFrequency { get; set; }
    }

}
