using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/plants")]
    public class PlantsController : ControllerBase
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public PlantsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
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
        public async Task<ActionResult<Plant>> CreatePlant([FromBody] Plant plant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            plant.PlantId = 0;

            _context.Plant.Add(plant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlant), new { id = plant.PlantId }, plant);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePlant(int id, [FromBody] Plant plant)
        {
            if (id != plant.PlantId)
                return BadRequest(new { message = "ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingPlant = await _context.Plant.FindAsync(id);
            if (existingPlant == null)
                return NotFound(new { message = "Plant not found." });

            existingPlant.PlantName = plant.PlantName;
            existingPlant.Type = plant.Type;
            existingPlant.GrowthTime = plant.GrowthTime;
            existingPlant.WaterFrequency = plant.WaterFrequency;
            existingPlant.isGrown = plant.isGrown;

            _context.Entry(plant).State = EntityState.Modified;

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
    }
}
