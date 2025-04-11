using Microsoft.EntityFrameworkCore;
using PlantsRPetsProjeto.Server.Data;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlantsRPetsProjeto.Tests.xUnitTests.ServiceTests
{
    public class MetricsServiceTests
    {
        private readonly DbContextOptions<PlantsRPetsProjetoServerContext> _options;
        private readonly string _testUserId = "test-user-id";
        private readonly int _testPlantationId = 1;
        private readonly int _testPlantInfoId = 1;

        public MetricsServiceTests()
        {
            _options = new DbContextOptionsBuilder<PlantsRPetsProjetoServerContext>()
                .UseInMemoryDatabase(databaseName: $"MetricsService_TestDb_{Guid.NewGuid()}")
                .Options;
        }

        private async Task SeedTestDataAsync()
        {
            using var context = new PlantsRPetsProjetoServerContext(_options);

            var user = new User
            {
                Id = _testUserId,
                UserName = "testuser@example.com",
                Email = "testuser@example.com",
                Nickname = "TestUser"
            };
            context.Users.Add(user);

            var plantType = new PlantType
            {
                PlantTypeId = 1,
                PlantTypeName = "Vegetable",
                HasRecurringHarvest = true
            };
            context.PlantType.Add(plantType);

            var plantInfo = new PlantInfo
            {
                PlantInfoId = _testPlantInfoId,
                PlantName = "Test Plant",
                PlantType = "Vegetable",
                Cycle = "Annual",
                Watering = "Frequent",
                PruningMonth = new List<string> { "March", "September" },
                GrowthRate = "Medium",
                Sunlight = new List<string> { "Full sun" },
                Edible = "Yes",
                CareLevel = "Easy",
                Flowers = "Yes",
                Fruits = "Yes",
                Leaf = true,
                Maintenance = "Low",
                SaltTolerant = "No",
                Indoor = false,
                FloweringSeason = "Spring",
                Description = "Test plant description",
                HarvestSeason = "Summer",
                ScientificName = new List<string> { "Testus plantus" },
                DroughtTolerant = false,
                Cuisine = true,
                Medicinal = false
            };
            context.PlantInfo.Add(plantInfo);

            var plantation = new Plantation
            {
                PlantationId = _testPlantationId,
                OwnerId = _testUserId,
                PlantationName = "Test Plantation",
                PlantTypeId = 1,
                PlantingDate = DateTime.UtcNow.AddMonths(-3),
                ExperiencePoints = 100,
                Level = 2,
                BankedLevelUps = 0
            };
            context.Plantation.Add(plantation);

            var plantationPlant = new PlantationPlants
            {
                PlantationPlantsId = 1,
                PlantationId = _testPlantationId,
                PlantInfoId = _testPlantInfoId,
                Quantity = 5,
                PlantingDate = DateTime.UtcNow.AddMonths(-3),
                LastWatered = DateTime.UtcNow.AddDays(-1),
                LastHarvested = DateTime.UtcNow.AddDays(-7),
                HarvestDate = DateTime.UtcNow.AddDays(14),
                GrowthStatus = "Growing"
            };
            context.PlantationPlants.Add(plantationPlant);

            var metrics = new List<Metric>
            {
                new Metric
                {
                    MetricId = 1,
                    UserId = _testUserId,
                    PlantationId = _testPlantationId,
                    PlantInfoId = _testPlantInfoId,
                    EventType = "Watering",
                    Timestamp = DateTime.UtcNow.AddDays(-1)
                },
                new Metric
                {
                    MetricId = 2,
                    UserId = _testUserId,
                    PlantationId = _testPlantationId,
                    PlantInfoId = _testPlantInfoId,
                    EventType = "Watering",
                    Timestamp = DateTime.UtcNow.AddDays(-3)
                },
                new Metric
                {
                    MetricId = 3,
                    UserId = _testUserId,
                    PlantationId = _testPlantationId,
                    PlantInfoId = _testPlantInfoId,
                    EventType = "Harvest",
                    Timestamp = DateTime.UtcNow.AddDays(-5)
                },
                new Metric
                {
                    MetricId = 4,
                    UserId = _testUserId,
                    PlantationId = _testPlantationId,
                    PlantInfoId = _testPlantInfoId,
                    EventType = "Planting",
                    Timestamp = DateTime.UtcNow.AddDays(-10)
                }
            };
            context.Metric.AddRange(metrics);

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task RecordWateringEventAsync_ShouldCreateMetric()
        {
            await using var context = new PlantsRPetsProjetoServerContext(_options);
            var service = new MetricsService(context);
            var timestamp = DateTime.UtcNow;

            await service.RecordWateringEventAsync(_testUserId, _testPlantationId, _testPlantInfoId, timestamp, 5);

            var metric = await context.Metric.FirstOrDefaultAsync(m =>
                m.UserId == _testUserId &&
                m.PlantationId == _testPlantationId &&
                m.PlantInfoId == _testPlantInfoId &&
                m.EventType == "Watering" &&
                m.Timestamp == timestamp);

            Assert.NotNull(metric);
            Assert.Equal("Watering", metric.EventType);
        }

        [Fact]
        public async Task RecordHarvestEventAsync_ShouldCreateMetric()
        {
            await using var context = new PlantsRPetsProjetoServerContext(_options);
            var service = new MetricsService(context);
            var timestamp = DateTime.UtcNow;

            await service.RecordHarvestEventAsync(_testUserId, _testPlantationId, _testPlantInfoId, timestamp, 5);

            var metric = await context.Metric.FirstOrDefaultAsync(m =>
                m.UserId == _testUserId &&
                m.PlantationId == _testPlantationId &&
                m.PlantInfoId == _testPlantInfoId &&
                m.EventType == "Harvest" &&
                m.Timestamp == timestamp);

            Assert.NotNull(metric);
            Assert.Equal("Harvest", metric.EventType);
        }

        [Fact]
        public async Task RecordPlantingEventAsync_ShouldCreateMetric()
        {
            await using var context = new PlantsRPetsProjetoServerContext(_options);
            var service = new MetricsService(context);
            var timestamp = DateTime.UtcNow;

            await service.RecordPlantingEventAsync(_testUserId, _testPlantationId, _testPlantInfoId, timestamp, 5);

            var metric = await context.Metric.FirstOrDefaultAsync(m =>
                m.UserId == _testUserId &&
                m.PlantationId == _testPlantationId &&
                m.PlantInfoId == _testPlantInfoId &&
                m.EventType == "Planting" &&
                m.Timestamp == timestamp);

            Assert.NotNull(metric);
            Assert.Equal("Planting", metric.EventType);
        }

        [Fact]
        public async Task GetActivityCountsAsync_ShouldReturnCorrectCounts()
        {
            await SeedTestDataAsync();
            await using var context = new PlantsRPetsProjetoServerContext(_options);
            var service = new MetricsService(context);

            var result = await service.GetActivityCountsAsync(_testUserId, "month");

            Assert.NotNull(result);
            Assert.True(result.ContainsKey("Watering"));
            Assert.True(result.ContainsKey("Harvest"));
            Assert.True(result.ContainsKey("Planting"));
            Assert.Equal(2, result["Watering"]);
            Assert.Equal(1, result["Harvest"]);
            Assert.Equal(1, result["Planting"]);
        }

        [Fact]
        public async Task GetActivityByDateAsync_ShouldReturnGroupedData()
        {
            await SeedTestDataAsync();
            await using var context = new PlantsRPetsProjetoServerContext(_options);
            var service = new MetricsService(context);

            var result = await service.GetActivityByDateAsync(_testUserId, "Watering", "month");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var firstItem = result[0];
            var dateProperty = firstItem.GetType().GetProperty("Date")?.GetValue(firstItem);
            var countProperty = firstItem.GetType().GetProperty("Count")?.GetValue(firstItem);

            Assert.NotNull(dateProperty);
            Assert.NotNull(countProperty);
        }

        [Fact]
        public async Task GetPlantTypeDistributionAsync_ShouldReturnDistributionData()
        {
            await SeedTestDataAsync();
            await using var context = new PlantsRPetsProjetoServerContext(_options);
            var service = new MetricsService(context);

            var result = await service.GetPlantTypeDistributionAsync(_testUserId);

            Assert.NotNull(result);
            Assert.Single(result);

            var firstItem = result[0];
            var plantTypeValue = firstItem.GetType().GetProperty("PlantType")?.GetValue(firstItem);
            var countValue = firstItem.GetType().GetProperty("Count")?.GetValue(firstItem);

            Assert.Equal("Vegetable", plantTypeValue);
            Assert.Equal(5, countValue);
        }

        [Theory]
        [InlineData("day", -1)]
        [InlineData("week", -7)]
        [InlineData("month", -30)]
        [InlineData("year", -365)]
        [InlineData("invalid", -7)]
        public void GetStartDateForTimeFrame_ShouldReturnCorrectDate(string timeFrame, int expectedDaysDifference)
        {
            using var context = new PlantsRPetsProjetoServerContext(_options);
            var service = new MetricsService(context);
            var nowUtc = DateTime.UtcNow;

            var methodInfo = typeof(MetricsService).GetMethod("GetStartDateForTimeFrame",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var result = (DateTime)methodInfo.Invoke(service, new object[] { timeFrame });

            var actualDaysDifference = (result - nowUtc).TotalDays;

            if (timeFrame == "day" || timeFrame == "week" || timeFrame == "invalid")
            {
                Assert.InRange(actualDaysDifference, expectedDaysDifference - 0.1, expectedDaysDifference + 0.1);
            }
            else if (timeFrame == "month")
            {
                Assert.InRange(actualDaysDifference, -31, -28);
            }
            else if (timeFrame == "year")
            {
                Assert.InRange(actualDaysDifference, -366, -364);
            }
        }
    }
}
