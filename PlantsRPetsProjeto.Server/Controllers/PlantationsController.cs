using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantationsController : ControllerBase
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public PlantationsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        // GET: api/Plantations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plantation>>> GetPlantations()
        {
            return await _context.Plantation.ToListAsync();
        }

        // GET: api/Plantations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plantation>> GetPlantation(int id)
        {
            var plantation = await _context.Plantation.FindAsync(id);
            if (plantation == null)
            {
                return NotFound(new { message = "Plantation not found." });
            }

            return Ok(plantation);
        }

        // POST: api/Plantations
        [HttpPost]
        public async Task<ActionResult<Plantation>> CreatePlantation([FromBody] Plantation plantation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            plantation.PlantingDate = DateTime.UtcNow;
            plantation.LastWatered = DateTime.UtcNow;
            plantation.ExperiencePoints = 0;
            plantation.Level = 1;
            plantation.GrowthStatus = "Growing"; 

            _context.Plantation.Add(plantation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlantation), new { id = plantation.PlantationId }, plantation);
        }

        // PUT: api/Plantations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlantation(int id, [FromBody] Plantation plantation)
        {
            if (id != plantation.PlantationId)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(plantation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantationExists(id))
                {
                    return NotFound(new { message = "Plantation not found." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Plantations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlantation(int id)
        {
            var plantation = await _context.Plantation.FindAsync(id);
            if (plantation == null)
            {
                return NotFound(new { message = "Plantation not found." });
            }

            _context.Plantation.Remove(plantation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Plantation deleted successfully." });
        }

        private bool PlantationExists(int id)
        {
            return _context.Plantation.Any(e => e.PlantationId == id);
        }
    }
}