using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;

namespace PlantsRPetsProjeto.Server.Controllers
{
    /// <summary>
    /// Controlador responsável por fornecer métricas relacionadas com a atividade do utilizador,
    /// como contagem de eventos, evolução temporal e distribuição por tipo de planta.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/metrics")]
    public class MetricsController : Controller
    {
        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly MetricsService _metricsService;

        // <summary>
        /// Construtor do controlador de métricas.
        /// </summary>
        /// <param name="context">Contexto da base de dados.</param>
        /// <param name="metricsService">Serviço responsável pelo cálculo e obtenção de métricas.</param>
        public MetricsController(PlantsRPetsProjetoServerContext context, MetricsService metricsService)
        {
            _context = context;
            _metricsService = metricsService;
        }

        /// <summary>
        /// Devolve a contagem de atividades realizadas pelo utilizador, agrupadas por tipo de evento.
        /// </summary>
        /// <param name="timeFrame">Período de tempo a considerar (ex: "week", "month").</param>
        /// <returns>Objeto com contagens de atividades por tipo.</returns>
        [HttpGet("metrics/activity-counts")]
        public async Task<IActionResult> GetActivityCounts([FromQuery] string timeFrame = "week")
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var counts = await _metricsService.GetActivityCountsAsync(userId, timeFrame);
            return Ok(counts);
        }

        /// <summary>
        /// Devolve os dados de atividade do utilizador ao longo do tempo, filtrados por tipo de evento.
        /// </summary>
        /// <param name="eventType">Tipo de evento (ex: "watering", "harvesting").</param>
        /// <param name="timeFrame">Período de tempo a considerar (ex: "week", "month").</param>
        /// <returns>Lista de atividades por data.</returns>
        [HttpGet("metrics/activity-by-date")]
        public async Task<IActionResult> GetActivityByDate([FromQuery] string eventType, [FromQuery] string timeFrame = "week")
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var data = await _metricsService.GetActivityByDateAsync(userId, eventType, timeFrame);
            return Ok(data);
        }

        /// <summary>
        /// Devolve a distribuição de plantações do utilizador por tipo de planta.
        /// </summary>
        /// <returns>Objeto com dados percentuais por tipo de planta.</returns>
        [HttpGet("metrics/plant-type-distribution")]
        public async Task<IActionResult> GetPlantTypeDistribution()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return NotFound(new { message = "User not found." });

            var data = await _metricsService.GetPlantTypeDistributionAsync(userId);
            return Ok(data);

        }
    }

}