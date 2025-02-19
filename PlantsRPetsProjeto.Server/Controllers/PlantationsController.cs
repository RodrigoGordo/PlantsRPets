using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/plantations")]
    public class PlantationsController : ControllerBase
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public PlantationsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plantation>>> GetUserPlantations()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var plantations = await _context.Plantation
                .Where(p => p.OwnerId == userId)
                .ToListAsync();

            return Ok(plantations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plantation>> GetPlantation(int id)
        {
            var plantation = await _context.Plantation
                .Include(p => p.PlantationPlants)
                .ThenInclude(pp => pp.ReferencePlant)
                .FirstOrDefaultAsync(p => p.PlantationId == id);

            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            return Ok(plantation);
        }

        [HttpPost]
        public async Task<ActionResult<Plantation>> CreatePlantation([FromBody] Plantation plantation)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            plantation.OwnerId = userId;
            plantation.PlantingDate = DateTime.UtcNow;
            plantation.LastWatered = DateTime.UtcNow;
            plantation.ExperiencePoints = 0;
            plantation.Level = 1;
            plantation.GrowthStatus = "Growing";
            plantation.PlantationPlants ??= new List<PlantationPlants>();

            _context.Plantation.Add(plantation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlantation), new { id = plantation.PlantationId }, plantation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlantation(int id, [FromBody] Plantation plantation)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            if (id != plantation.PlantationId)
                return BadRequest(new { message = "ID mismatch." });

            var existingPlantation = await _context.Plantation.FindAsync(id);
            if (existingPlantation == null)
                return NotFound(new { message = "Plantation not found." });

            if (existingPlantation.OwnerId != userId)
                return Forbid("You do not have permission to update this plantation.");

            existingPlantation.PlantationName = plantation.PlantationName;
            existingPlantation.PlantType = plantation.PlantType;
            existingPlantation.LastWatered = plantation.LastWatered;
            existingPlantation.HarvestDate = plantation.HarvestDate;
            existingPlantation.GrowthStatus = plantation.GrowthStatus;
            existingPlantation.ExperiencePoints = plantation.ExperiencePoints;
            existingPlantation.Level = plantation.Level;

            _context.Entry(plantation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Conflict updating plantation, please try again." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlantation(int id)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var plantation = await _context.Plantation.FindAsync(id);
            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            if (plantation.OwnerId != userId)
                return Forbid("You do not have permission to delete this plantation.");

            _context.Plantation.Remove(plantation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Plantation deleted successfully." });
        }

        [HttpPost("{plantationId}/add-plant")]
        public async Task<IActionResult> AddPlantToPlantation(int plantationId, [FromBody] PlantationPlants plantationPlant)
        {
            var plantation = await _context.Plantation.FindAsync(plantationId);
            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            var plant = await _context.Plant.FindAsync(plantationPlant.PlantId);
            if (plant == null)
                return NotFound(new { message = "Plant not found." });

            if (plant.Type != plantation.PlantType)
                return BadRequest(new { message = "This plant type is not allowed in this plantation." });

            var existingPlant = await _context.PlantationPlants
                .FirstOrDefaultAsync(pp => pp.PlantationId == plantationId && pp.PlantId == plantationPlant.PlantId);

            if (existingPlant != null)
                existingPlant.Quantity += plantationPlant.Quantity;
            else
            {
                plantationPlant.PlantationId = plantationId;
                _context.PlantationPlants.Add(plantationPlant);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Plant added to plantation successfully." });
        }

        [HttpDelete("{plantationId}/remove-plant/{plantId}")]
        public async Task<IActionResult> RemovePlantFromPlantation(int plantationId, int plantId)
        {
            var plantationPlant = await _context.PlantationPlants
                .FirstOrDefaultAsync(pp => pp.PlantationId == plantationId && pp.PlantId == plantId);

            if (plantationPlant == null)
                return NotFound(new { message = "Plant not found in this plantation." });

            _context.PlantationPlants.Remove(plantationPlant);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Plant removed from plantation successfully." });
        }

        [HttpGet("{plantationId}/plants")]
        public async Task<ActionResult<IEnumerable<PlantationPlants>>> GetPlantsInPlantation(int plantationId)
        {
            var plants = await _context.PlantationPlants
                .Where(pp => pp.PlantationId == plantationId)
                .Include(pp => pp.ReferencePlant)
                .ToListAsync();

            return Ok(plants);
        }

        private bool PlantationExists(int id)
        {
            return _context.Plantation.Any(e => e.PlantationId == id);
        }
    }
}