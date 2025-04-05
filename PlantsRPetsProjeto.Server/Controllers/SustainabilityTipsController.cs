using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;

namespace PlantsRPetsProjeto.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SustainabilityTipsController : ControllerBase
    {
        private readonly SustainabilityTipService _tipsService;
        private readonly PlantsRPetsProjetoServerContext _context;

        public SustainabilityTipsController(
            SustainabilityTipService tipsService,
            PlantsRPetsProjetoServerContext context)
        {
            _tipsService = tipsService;
            _context = context;
        }

        [HttpGet("fetch-range/{startId}/{maxId}")]
        public async Task<IActionResult> FetchAndStoreSustainabilityTips(int startId, int maxId)
        {
            if (startId <= 0 || maxId <= 0 || startId > maxId)
                return BadRequest("Invalid ID range");

            try
            {
                var tipsLists = await _tipsService.GetSustainabilityTipsAsync(startId, maxId);

                if (tipsLists?.Count == 0)
                    return NotFound("No tips found in this range");

                await SaveSustainabilityTips(tipsLists);
                return Ok(tipsLists);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, $"API Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }

        [HttpPost("save-sustainability-tips")]
        public async Task<IActionResult> SaveSustainabilityTips(List<SustainabilityTipsList> tipsLists)
        {
            foreach (var tipsList in tipsLists)
            {
                var existingList = await _context.SustainabilityTipsList
                    .Include(l => l.SustainabilityTip)
                    .FirstOrDefaultAsync(l => l.PlantInfoId == tipsList.PlantInfoId);

                if (existingList != null)
                {
                    existingList.PlantName = tipsList.PlantName;
                    existingList.PlantScientificName = tipsList.PlantScientificName;
                    _context.SustainabilityTip.RemoveRange(existingList.SustainabilityTip);
                    existingList.SustainabilityTip = tipsList.SustainabilityTip;

                    _context.Update(existingList);
                }
                else
                {
                    _context.Add(tipsList);
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SustainabilityTipsList>>> GetAllSustainabilityTips()
        {
            return await _context.SustainabilityTipsList
                .Include(l => l.SustainabilityTip)
                .ToListAsync();
        }

        [HttpGet("by-plant/{plantId}")]
        public async Task<ActionResult<IEnumerable<TipDto>>> GetTipsByPlant(int plantId)
        {
            var query = from tipsList in _context.SustainabilityTipsList
                        where tipsList.PlantInfoId == plantId
                        join tip in _context.SustainabilityTip
                            on tipsList.SustainabilityTipsListId equals tip.SustainabilityTipsListId
                        select new TipDto
                        {
                            TipId = tip.SustainabilityTipId,
                            PlantInfoId = tipsList.PlantInfoId,
                            TipDescription = tip.Description,
                            TipType = tip.Type
                        };

            var results = await query.ToListAsync();

            if (!results.Any())
            {
                return NotFound("No tips found for this plant");
            }

            return Ok(results);
        }
    }
    public class TipDto
    {
        public int TipId { get; set; }
        public int PlantInfoId { get; set; }
        public string TipDescription { get; set; }
        public string TipType { get; set; }
    }
}
