using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return await _context.Plant.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(int id)
        {
            var plant = await _context.Plant.FindAsync(id);
            if (plant == null)
            {
                return NotFound(new { message = "Plant not found." });
            }
            return Ok(plant);
        }

        [HttpPost]
        public async Task<ActionResult<Plant>> CreatePlant([FromBody] Plant plant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Plant.Add(plant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlant), new { id = plant.PlantId }, plant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlant(int id, [FromBody] Plant plant)
        {
            if (id != plant.PlantId)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(plant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Plant.Any(e => e.PlantId == id))
                {
                    return NotFound(new { message = "Plant not found." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            var plant = await _context.Plant.FindAsync(id);
            if (plant == null)
            {
                return NotFound(new { message = "Plant not found." });
            }

            _context.Plant.Remove(plant);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Plant deleted successfully." });
        }
    }
}
