using System.ComponentModel.DataAnnotations;
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
    [Route("api/plantations")]
    public class PlantationsController : ControllerBase
    {
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly PlantInfoService _plantInfoService;

        public PlantationsController(PlantsRPetsProjetoServerContext context, PlantInfoService plantInfoService)
        {
            _context = context;
            _plantInfoService = plantInfoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUserPlantations()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            await _plantInfoService.PopulatePlantTypesAsync();

            var plantations = await _context.Plantation
                .Where(p => p.OwnerId == userId)
                .Join(
                    _context.PlantType,
                    plantation => plantation.PlantTypeId,
                    plantType => plantType.PlantTypeId,
                    (plantation, plantType) => new
                    {
                        plantation.PlantationId,
                        plantation.OwnerId,
                        plantation.PlantationName,
                        plantation.PlantTypeId,
                        plantType.PlantTypeName,
                        plantation.PlantingDate,
                        plantation.LastWatered,
                        plantation.HarvestDate,
                        plantation.GrowthStatus,
                        plantation.ExperiencePoints,
                        plantation.Level
                    }
                )
                .ToListAsync();

            return Ok(plantations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plantation>> GetPlantation(int id)
        {
            var plantation = await _context.Plantation
                //.Include(p => p.PlantType)
                .Include(p => p.PlantationPlants)
                .ThenInclude(pp => pp.ReferencePlant)
                .FirstOrDefaultAsync(p => p.PlantationId == id);

            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            return Ok(plantation);
        }

        [HttpPost]
        public async Task<ActionResult<Plantation>> CreatePlantation([FromBody] CreatePlantationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var plantType = await _context.PlantType.FindAsync(model.PlantTypeId);
            if (plantType == null)
                return NotFound(new { message = "Plant type not found." });

            var plantation = new Plantation
            {
                OwnerId = userId,
                PlantationName = model.PlantationName,
                PlantTypeId = model.PlantTypeId,
                PlantingDate = DateTime.UtcNow,
                LastWatered = DateTime.UtcNow,
                GrowthStatus = "Growing",
                ExperiencePoints = 0,
                Level = 1,
                PlantationPlants = [],
                HarvestDate = DateTime.UtcNow
            };

            _context.Plantation.Add(plantation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlantation), new { id = plantation.PlantationId }, plantation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlantation(int id, [FromBody] UpdatePlantationModel model)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var existingPlantation = await _context.Plantation.FindAsync(id);
            if (existingPlantation == null)
                return NotFound(new { message = "Plantation not found." });

            if (existingPlantation.OwnerId != userId)
                return Forbid("You do not have permission to update this plantation.");

            if (model.PlantationName != null)
                existingPlantation.PlantationName = model.PlantationName;

            if (model.PlantTypeId.HasValue)
            {
                var plantType = await _context.PlantType.FindAsync(model.PlantTypeId.Value);
                if (plantType == null)
                    return NotFound(new { message = "Plant type not found." });

                existingPlantation.PlantTypeId = model.PlantTypeId.Value;
            }

            if (model.LastWatered.HasValue)
                existingPlantation.LastWatered = model.LastWatered.Value;

            if (model.HarvestDate.HasValue)
                existingPlantation.HarvestDate = model.HarvestDate.Value;

            if (model.GrowthStatus != null)
                existingPlantation.GrowthStatus = model.GrowthStatus;

            if (model.ExperiencePoints.HasValue)
                existingPlantation.ExperiencePoints = model.ExperiencePoints.Value;

            if (model.Level.HasValue)
                existingPlantation.Level = model.Level.Value;

            _context.Entry(existingPlantation).State = EntityState.Modified;

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

            try
            {
                _context.Plantation.Remove(plantation);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Plantation deleted successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Plantation was already removed or updated by another process." });
            }
        }

        [HttpPost("{plantationId}/add-plant")]
        public async Task<IActionResult> AddPlantToPlantation(int plantationId, [FromBody] AddPlantToPlantationModel model)
        {
            var plantation = await _context.Plantation.FindAsync(plantationId);
            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            var plant = await _context.PlantInfo.FindAsync(model.PlantInfoId);
            if (plant == null)
                return NotFound(new { message = "Plant not found." });

            var plantType = await _context.PlantType.FindAsync(plantation.PlantTypeId);
            if (plantType == null)
                return NotFound(new { message = "Plant Type not found." });

            if (plant.PlantType != plantType.PlantTypeName)
                return BadRequest(new { message = "This plant type is not allowed in this plantation." });

            if (model.Quantity <= 0)
                return BadRequest(new { message = "Quantity must be greater than zero." });

            var existingPlant = await _context.PlantationPlants
                .FirstOrDefaultAsync(pp => pp.PlantationId == plantationId && pp.PlantInfoId == model.PlantInfoId);

            if (existingPlant != null)
            {
                existingPlant.Quantity += model.Quantity;
            }
            else
            {
                var plantationPlant = new PlantationPlants
                {
                    PlantationId = plantationId,
                    PlantInfoId = model.PlantInfoId,
                    Quantity = model.Quantity
                };

                _context.PlantationPlants.Add(plantationPlant);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Plant added to plantation successfully." });
        }

        [HttpDelete("{plantationId}/remove-plant/{plantId}")]
        public async Task<IActionResult> RemovePlantFromPlantation(int plantationId, int plantId, [FromBody] RemovePlantFromPlantationModel model)
        {
            var plantation = await _context.Plantation.FindAsync(plantationId);
            if (plantation == null)
                return NotFound(new { message = "Plantation not found." });

            var plant = await _context.PlantInfo.FindAsync(plantId);
            if (plant == null)
                return NotFound(new { message = "Plant not found." });

            var plantationPlant = await _context.PlantationPlants
                .FirstOrDefaultAsync(pp => pp.PlantationId == plantationId && pp.PlantInfoId == plantId);

            if (plantationPlant == null)
                return NotFound(new { message = "Plant not found in this plantation." });

            if (model.Quantity.HasValue)
            {
                if (model.Quantity.Value <= 0)
                    return BadRequest(new { message = "Quantity must be greater than zero." });

                plantationPlant.Quantity -= model.Quantity.Value;

                if (plantationPlant.Quantity <= 0)
                {
                    _context.PlantationPlants.Remove(plantationPlant);
                }
            }
            else
            {
                _context.PlantationPlants.Remove(plantationPlant);
            }

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

    }

    public class CreatePlantationModel
    {
        public required string PlantationName { get; set; }
        public required int PlantTypeId { get; set; }
    }

    public class UpdatePlantationModel
    {
        public string? PlantationName { get; set; }
        public DateTime? LastWatered { get; set; }
        public DateTime? HarvestDate { get; set; }
        public int? PlantTypeId { get; set; }
        public string? GrowthStatus { get; set; }
        public int? ExperiencePoints { get; set; }
        public int? Level { get; set; }
    }

    public class AddPlantToPlantationModel
    {
        public int PlantInfoId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }

    public class RemovePlantFromPlantationModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int? Quantity { get; set; }
    }
}