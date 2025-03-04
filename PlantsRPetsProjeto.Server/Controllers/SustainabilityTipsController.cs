using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [ApiController]
    [Route("api/sustainability-tips")]
    public class SustainabilityTipsController : ControllerBase
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public SustainabilityTipsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        // GET: api/sustainability-tips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SustainabilityTip>>> GetSustainabilityTips()
        {
            var tips = await _context.SustainabilityTip.ToListAsync();
            return Ok(tips); // Return the list wrapped in an OkObjectResult
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SustainabilityTip>> GetSustainabilityTip(int id)
        {
            var tip = await _context.SustainabilityTip.FindAsync(id);

            if (tip == null)
            {
                return NotFound();
            }

            return Ok(tip); // Return the tip wrapped in an OkObjectResult
        }


        [HttpPost]
        public async Task<ActionResult<SustainabilityTip>> CreateSustainabilityTip(SustainabilityTip tip)
        {
            if (tip == null || string.IsNullOrWhiteSpace(tip.Title) || string.IsNullOrWhiteSpace(tip.Content))
            {
                return BadRequest("Invalid tip data.");
            }

            _context.SustainabilityTip.Add(new SustainabilityTip
            {
                Title = tip.Title,
                Content = tip.Content,
                Category = tip.Category,
                AuthorId = tip.AuthorId

            });

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSustainabilityTip), new { id = tip.Id }, tip);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSustainabilityTip(int id, SustainabilityTip tip)
        {
            if (tip == null || string.IsNullOrWhiteSpace(tip.Title) || string.IsNullOrWhiteSpace(tip.Content))
            {
                return BadRequest("Invalid tip data.");
            }

            var existingTip = await _context.SustainabilityTip.FindAsync(id);
            if (existingTip == null)
            {
                return NotFound();
            }

            existingTip.Title = tip.Title;
            existingTip.Content = tip.Content;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "A concurrency issue occurred while updating the tip.");
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSustainabilityTip(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            var tip = await _context.SustainabilityTip.FindAsync(id);
            if (tip == null)
            {
                return NotFound();
            }

            _context.SustainabilityTip.Remove(tip);
            await _context.SaveChangesAsync();

            return NoContent(); // Returning NoContent after deletion
        }

        private bool SustainabilityTipExists(int id)
        {
            return _context.SustainabilityTip.Any(e => e.Id == id);
        }
    }
}
