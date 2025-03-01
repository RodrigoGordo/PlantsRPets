using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Authorize]
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
            return await _context.SustainabilityTip.ToListAsync();
        }

        // GET: api/sustainability-tips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SustainabilityTip>> GetSustainabilityTip(int id)
        {
            var tip = await _context.SustainabilityTip.FindAsync(id);

            if (tip == null)
            {
                return NotFound();
            }

            return tip;
        }

        // POST: api/sustainability-tips
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SustainabilityTip>> CreateSustainabilityTip(SustainabilityTip tip)
        {
            _context.SustainabilityTip.Add(tip);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSustainabilityTip), new { id = tip.Id }, tip);
        }

        // PUT: api/sustainability-tips/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSustainabilityTip(int id, SustainabilityTip tip)
        {
            if (id != tip.Id)
            {
                return BadRequest();
            }

            _context.Entry(tip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SustainabilityTipExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/sustainability-tips/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSustainabilityTip(int id)
        {
            var tip = await _context.SustainabilityTip.FindAsync(id);
            if (tip == null)
            {
                return NotFound();
            }

            _context.SustainabilityTip.Remove(tip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SustainabilityTipExists(int id)
        {
            return _context.SustainabilityTip.Any(e => e.Id == id);
        }
    }
}
