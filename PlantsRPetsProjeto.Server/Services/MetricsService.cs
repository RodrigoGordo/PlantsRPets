using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Services
{
    /// <summary>
    /// Serviço responsável pelo registo de eventos de atividade dos utilizadores,
    /// bem como pela geração de métricas e estatísticas associadas às plantações.
    /// </summary>
    public class MetricsService
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="MetricsService"/>.
        /// </summary>
        /// <param name="context">Contexto da base de dados da aplicação.</param>
        public MetricsService(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Regista um evento de rega no histórico de métricas para uma planta específica numa plantação.
        /// </summary>
        /// <param name="userId">ID do utilizador que realizou a ação.</param>
        /// <param name="plantationId">ID da plantação onde a planta está localizada.</param>
        /// <param name="plantInfoId">ID da planta que foi regada.</param>
        /// <param name="timestamp">Data e hora em que o evento ocorreu.</param>

        public async Task RecordWateringEventAsync(string userId, int plantationId, int plantInfoId, DateTime timestamp)
        {
            var metric = new Metric
            {
                UserId = userId,
                PlantationId = plantationId,
                PlantInfoId = plantInfoId,
                EventType = "Watering",
                Timestamp = timestamp
            };

            _context.Metric.Add(metric);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Regista um evento de colheita no histórico de métricas para uma planta específica numa plantação.
        /// </summary>
        /// <param name="userId">ID do utilizador que realizou a colheita.</param>
        /// <param name="plantationId">ID da plantação onde a planta está localizada.</param>
        /// <param name="plantInfoId">ID da planta que foi colhida.</param>
        /// <param name="timestamp">Data e hora da colheita.</param>

        public async Task RecordHarvestEventAsync(string userId, int plantationId, int plantInfoId, DateTime timestamp)
        {
            var metric = new Metric
            {
                UserId = userId,
                PlantationId = plantationId,
                PlantInfoId = plantInfoId,
                EventType = "Harvest",
                Timestamp = timestamp
            };

            _context.Metric.Add(metric);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Regista um ou mais eventos de plantação no histórico de métricas para uma planta específica.
        /// </summary>
        /// <param name="userId">ID do utilizador que realizou a plantação.</param>
        /// <param name="plantationId">ID da plantação onde as plantas foram adicionadas.</param>
        /// <param name="plantInfoId">ID da planta adicionada.</param>
        /// <param name="timestamp">Data e hora da plantação.</param>
        /// <param name="quantity">Número de unidades da planta adicionadas. Cada unidade será registada individualmente como evento.</param>
        public async Task RecordPlantingEventAsync(string userId, int plantationId, int plantInfoId, DateTime timestamp, int quantity)
        {
            for(int i = 0; i < quantity; i++)
            {
                var metric = new Metric
                {
                    UserId = userId,
                    PlantationId = plantationId,
                    PlantInfoId = plantInfoId,
                    EventType = "Planting",
                    Timestamp = timestamp
                };
                _context.Metric.Add(metric);
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtém a contagem de eventos (rega, colheita, plantação) realizados por um utilizador num determinado intervalo temporal.
        /// </summary>
        /// <param name="userId">ID do utilizador.</param>
        /// <param name="timeFrame">Intervalo de tempo (ex: "day", "week", "month", "year").</param>
        /// <returns>Dicionário com a contagem de eventos por tipo.</returns>
        public async Task<Dictionary<string, int>> GetActivityCountsAsync(string userId, string timeFrame)
        {
            DateTime startDate = GetStartDateForTimeFrame(timeFrame);

            var metrics = await _context.Metric
                .Where(m => m.UserId == userId && m.Timestamp >= startDate)
                .GroupBy(m => m.EventType)
                .Select(g => new { EventType = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new Dictionary<string, int>
            {
                ["Watering"] = metrics.FirstOrDefault(m => m.EventType == "Watering")?.Count ?? 0,
                ["Harvest"] = metrics.FirstOrDefault(m => m.EventType == "Harvest")?.Count ?? 0,
                ["Planting"] = metrics.FirstOrDefault(m => m.EventType == "Planting")?.Count ?? 0
            };

            return result;
        }

        /// <summary>
        /// Devolve a contagem de eventos de um determinado tipo agrupados por data, consoante o intervalo de tempo escolhido.
        /// </summary>
        /// <param name="userId">ID do utilizador.</param>
        /// <param name="eventType">Tipo de evento (ex: "Watering").</param>
        /// <param name="timeFrame">Intervalo de tempo (ex: "day", "week", "month", "year").</param>
        /// <returns>Lista de objetos com a data formatada e número de eventos ocorridos.</returns>
        public async Task<List<object>> GetActivityByDateAsync(string userId, string eventType, string timeFrame)
        {
            DateTime startDate = GetStartDateForTimeFrame(timeFrame);

            var groupingFormat = timeFrame.ToLower() switch
            {
                "day" => "yyyy-MM-dd HH:00:00",
                "week" => "yyyy-MM-dd",
                "month" => "yyyy-MM-dd",
                "year" => "yyyy-MM",
                _ => "yyyy-MM-dd"
            };

            var metrics = await _context.Metric
                .Where(m => m.UserId == userId && m.EventType == eventType && m.Timestamp >= startDate)
                .ToListAsync();

            var groupedMetrics = metrics
                .GroupBy(m => m.Timestamp.ToString(groupingFormat))
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToList();

            return groupedMetrics.Cast<object>().ToList();
        }

        /// <summary>
        /// Calcula a distribuição de tipos de planta nas plantações do utilizador,
        /// com base na quantidade total de plantas de cada tipo.
        /// </summary>
        /// <param name="userId">ID do utilizador.</param>
        /// <returns>Lista de objetos com o tipo de planta e total de unidades plantadas.</returns>
        public async Task<List<object>> GetPlantTypeDistributionAsync(string userId)
        {
            var plantations = await _context.Plantation
                .Where(p => p.OwnerId == userId)
                .Join(
                    _context.PlantationPlants,
                    p => p.PlantationId,
                    pp => pp.PlantationId,
                    (p, pp) => new { Plantation = p, PlantationPlant = pp }
                )
                .Join(
                    _context.PlantInfo,
                    combined => combined.PlantationPlant.PlantInfoId,
                    pi => pi.PlantInfoId,
                    (combined, pi) => new { combined.Plantation, combined.PlantationPlant, PlantInfo = pi }
                )
                .GroupBy(x => x.PlantInfo.PlantType)
                .Select(g => new { PlantType = g.Key, Count = g.Sum(x => x.PlantationPlant.Quantity) })
                .ToListAsync();

            return plantations.Cast<object>().ToList();
        }

        /// <summary>
        /// Devolve a data de início do intervalo a considerar para análise, com base no parâmetro fornecido.
        /// </summary>
        /// <param name="timeFrame">Intervalo (ex: "day", "week", "month", "year").</param>
        /// <returns>Data inicial correspondente.</returns>
        private DateTime GetStartDateForTimeFrame(string timeFrame)
        {
            var now = DateTime.UtcNow;
            return timeFrame.ToLower() switch
            {
                "day" => now.AddHours(-24),
                "week" => now.AddDays(-7),
                "month" => now.AddMonths(-1),
                "year" => now.AddYears(-1),
                _ => now.AddDays(-7) // Default to week
            };
        }
    }
}