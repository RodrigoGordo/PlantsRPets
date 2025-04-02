using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Services
{
    public class MetricsService
    {
        private readonly PlantsRPetsProjetoServerContext _context;

        public MetricsService(PlantsRPetsProjetoServerContext context)
        {
            _context = context;
        }

        public async Task RecordWateringAsync(string userId, int plantationId, int plantTypeId)
        {
            var today = DateTime.UtcNow.Date;
            var metric = await GetOrCreateDailyMetric(userId, plantationId, plantTypeId, today);

            metric.WateringCount++;
            await _context.SaveChangesAsync();
        }

        public async Task RecordHarvestAsync(string userId, int plantationId, int plantTypeId)
        {
            var today = DateTime.UtcNow.Date;
            var metric = await GetOrCreateDailyMetric(userId, plantationId, plantTypeId, today);

            metric.HarvestCount++;
            await _context.SaveChangesAsync();
        }

        public async Task RecordPlantingAsync(string userId, int plantationId, int plantTypeId)
        {
            var today = DateTime.UtcNow.Date;
            var metric = await GetOrCreateDailyMetric(userId, plantationId, plantTypeId, today);

            metric.PlantingCount++;
            await _context.SaveChangesAsync();
        }

        private async Task<Metric> GetOrCreateDailyMetric(string userId, int plantationId, int plantTypeId, DateTime date)
        {
            var metric = await _context.Metric
                .FirstOrDefaultAsync(m =>
                    m.UserId == userId &&
                    m.PlantationId == plantationId &&
                    m.Date.Date == date.Date);

            if (metric == null)
            {
                metric = new Metric
                {
                    UserId = userId,
                    PlantationId = plantationId,
                    PlantTypeId = plantTypeId,
                    Date = date,
                    WateringCount = 0,
                    HarvestCount = 0,
                    PlantingCount = 0
                };

                _context.Metric.Add(metric);
            }

            return metric;
        }

        public async Task<object> GetUserMetricsSummaryAsync(string userId, int days = 30)
        {
            var startDate = DateTime.UtcNow.Date.AddDays(-days);

            var totals = await _context.Metric
                .Where(m => m.UserId == userId && m.Date >= startDate)
                .GroupBy(m => 1)
                .Select(g => new
                {
                    TotalWatering = g.Sum(m => m.WateringCount),
                    TotalHarvest = g.Sum(m => m.HarvestCount),
                    TotalPlanting = g.Sum(m => m.PlantingCount)
                })
                .FirstOrDefaultAsync() ?? new { TotalWatering = 0, TotalHarvest = 0, TotalPlanting = 0 };

            var dailyMetrics = await _context.Metric
                .Where(m => m.UserId == userId && m.Date >= startDate)
                .GroupBy(m => m.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    WateringCount = g.Sum(m => m.WateringCount),
                    HarvestCount = g.Sum(m => m.HarvestCount),
                    PlantingCount = g.Sum(m => m.PlantingCount)
                })
                .OrderBy(m => m.Date)
                .ToListAsync();

            var plantTypeDistribution = await _context.Metric
                .Where(m => m.UserId == userId && m.Date >= startDate && m.PlantTypeId != null)
                .GroupBy(m => m.PlantTypeId)
                .Select(g => new
                {
                    PlantTypeId = g.Key,
                    PlantingCount = g.Sum(m => m.PlantingCount)
                })
                .ToListAsync();

            var plantTypeDetails = await Task.WhenAll(plantTypeDistribution.Select(async p =>
            {
                var plantType = await _context.PlantType.FindAsync(p.PlantTypeId);
                return new
                {
                    TypeId = p.PlantTypeId,
                    TypeName = plantType?.PlantTypeName ?? "Unknown",
                    Count = p.PlantingCount
                };
            }));

            return new
            {
                Summary = totals,
                Daily = dailyMetrics,
                PlantTypes = plantTypeDetails
            };
        }
    }
}