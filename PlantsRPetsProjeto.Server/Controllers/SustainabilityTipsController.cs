using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;

namespace PlantsRPetsProjeto.Server.Controllers
{
    /// <summary>
    /// Controlador responsável por gerir dicas de sustentabilidade associadas a plantas.
    /// Permite buscar dicas de uma API externa, guardar e consultar dicas por planta.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SustainabilityTipsController : ControllerBase
    {
        private readonly SustainabilityTipService _tipsService;
        private readonly PlantsRPetsProjetoServerContext _context;

        /// <summary>
        /// Construtor do controlador de dicas de sustentabilidade.
        /// </summary>
        /// <param name="tipsService">Serviço que comunica com a API externa de dicas.</param>
        /// <param name="context">Contexto da base de dados.</param>
        public SustainabilityTipsController(SustainabilityTipService tipsService, PlantsRPetsProjetoServerContext context)
        {
            _tipsService = tipsService;
            _context = context;
        }
        /// <summary>
        /// Procura dicas de sustentabilidade numa API externa, dentro de um intervalo de IDs, e guarda-as na base de dados.
        /// </summary>
        /// <param name="startId">ID inicial do intervalo.</param>
        /// <param name="maxId">ID final do intervalo.</param>
        /// <returns>Lista de dicas encontradas ou mensagem de erro apropriada.</returns>
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

        /// <summary>
        /// Guarda uma lista de dicas de sustentabilidade na base de dados.
        /// Substitui os registos existentes e respetivas dicas, se já houver correspondência pelo ID da planta.
        /// </summary>
        /// <param name="tipsLists">Lista de objetos com dicas agrupadas por planta.</param>
        /// <returns>Resultado da operação de armazenamento.</returns>
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

        /// <summary>
        /// Devolve todas as listas de dicas de sustentabilidade armazenadas na base de dados.
        /// </summary>
        /// <returns>Lista completa de dicas organizadas por planta.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SustainabilityTipsList>>> GetAllSustainabilityTips()
        {
            return await _context.SustainabilityTipsList
                .Include(l => l.SustainabilityTip)
                .ToListAsync();
        }

        /// <summary>
        /// Devolve todas as dicas de sustentabilidade associadas a uma planta específica.
        /// </summary>
        /// <param name="plantId">Identificador da planta.</param>
        /// <returns>Lista de dicas associadas ou uma mensagem de erro caso não existam.</returns>
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

    /// <summary>
    /// Objeto de transferência de dados (DTO) para representar uma dica de sustentabilidade individual.
    /// </summary>
    public class TipDto
    {
        public int TipId { get; set; }
        public int PlantInfoId { get; set; }
        public string TipDescription { get; set; }
        public string TipType { get; set; }
    }
}
