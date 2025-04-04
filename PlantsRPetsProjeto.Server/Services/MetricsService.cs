using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Services
{
    public class MetricsService
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public MetricsService(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

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

        public async Task RecordPlantingEventAsync(string userId, int plantationId, int plantInfoId, DateTime timestamp)
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
            await _context.SaveChangesAsync();
        }

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