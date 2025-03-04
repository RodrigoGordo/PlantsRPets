using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [ApiController]
    [Route("api/tutorials")]
    public class TutorialsController : ControllerBase
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public TutorialsController(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        // GET: api/tutorials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tutorial>>> GetTutorials()
        {
            var tutorials = await _context.Tutorial.ToListAsync();
            return Ok(tutorials);
        }

        // GET: api/tutorials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tutorial>> GetTutorial(int id)
        {
            var tutorial = await _context.Tutorial.FindAsync(id);

            if (tutorial == null)
            {
                return NotFound();
            }

            return Ok(tutorial);
        }

        // POST: api/tutorials
        [HttpPost]
        public async Task<ActionResult<Tutorial>> CreateTutorial(Tutorial tutorial)
        {
            if (tutorial == null || string.IsNullOrWhiteSpace(tutorial.Title) || string.IsNullOrWhiteSpace(tutorial.Content))
            {
                return BadRequest("Invalid tutorial data."); // Return BadRequest if invalid data
            }

            _context.Tutorial.Add(tutorial);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTutorial), new { id = tutorial.Id }, tutorial); // Return CreatedAtAction with tutorial details
        }

        // PUT: api/tutorials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTutorial(int id, Tutorial tutorial)
        {
            if (tutorial == null)
            {
                return BadRequest("Invalid tutorial data.");
            }

            var existingTutorial = await _context.Tutorial.FindAsync(id);
            if (existingTutorial == null)
            {
                return NotFound();
            }

            existingTutorial.Title = tutorial.Title;
            existingTutorial.Content = tutorial.Content;
            existingTutorial.AuthorId = tutorial.AuthorId;

            _context.Entry(existingTutorial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "A concurrency error occurred while updating the tutorial.");
            }

            return NoContent();
        }

        // DELETE: api/tutorials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutorial(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID."); // Return BadRequest for invalid ID
            }

            var tutorial = await _context.Tutorial.FindAsync(id);
            if (tutorial == null)
            {
                return NotFound(); // Return NotFound if tutorial does not exist
            }

            _context.Tutorial.Remove(tutorial);
            await _context.SaveChangesAsync();

            return NoContent(); // Return NoContent after successful deletion
        }

        private bool TutorialExists(int id)
        {
            return _context.Tutorial.Any(e => e.Id == id);
        }
    }
}
