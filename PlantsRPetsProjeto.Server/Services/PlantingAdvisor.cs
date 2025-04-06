using PlantsRPetsProjeto.Server.Models;
using System.Globalization;

namespace PlantsRPetsProjeto.Server.Services
{
    public class PlantingAdvisor
    {
        private static readonly Dictionary<string, Dictionary<string, int>> GrowthTable = new()
        {
            { "tree", new() { { "High", 24 }, { "Moderate", 36 }, { "Low", 48 } } },
            { "flower", new() { { "High", 3 }, { "Moderate", 6 }, { "Low", 9 } } },
            { "herb", new() { { "High", 2 }, { "Moderate", 4 }, { "Low", 6 } } },
            { "succulent", new() { { "High", 4 }, { "Moderate", 8 }, { "Low", 12 } } },
            { "shrub", new() { { "High", 6 }, { "Moderate", 12 }, { "Low", 18 } } },
            { "vegetable", new() { { "High", 2 }, { "Moderate", 4 }, { "Low", 6 } } },
            { "vine", new() { { "High", 3 }, { "Moderate", 6 }, { "Low", 9 } } }
        };

        private static readonly Dictionary<string, Dictionary<string, int>> WateringTable = new()
        {
            { "Tree", new() { { "Minimal", 156 }, { "Average", 84 }, { "Frequent", 60 } } },
            { "Shrub", new() { { "Minimal", 144 }, {"Average", 84 }, { "Frequent", 36 } } },
            { "Vegetable", new(){ { "Minimal", 48 }, { "Average", 36 },{ "Frequent", 24 } } }

        };

        private static readonly Dictionary<string, Dictionary<string, GrowthData>> HarvestTable = new()
        {
            { "tree", new()
                {
                    { "high", new GrowthData { MaturityMonths = 18, HarvestOffsetMonths = 12 } },
                    { "moderate", new GrowthData { MaturityMonths = 24, HarvestOffsetMonths = 12 } },
                    { "low", new GrowthData { MaturityMonths = 36, HarvestOffsetMonths = 12 } },
                }
            },
            { "shrub", new()
                {
                    { "high", new GrowthData { MaturityMonths = 12, HarvestOffsetMonths = 6 } },
                    { "moderate", new GrowthData { MaturityMonths = 18, HarvestOffsetMonths = 6 } },
                    { "low", new GrowthData { MaturityMonths = 24, HarvestOffsetMonths = 6 } },
                }
            },
            { "vine", new()
                {
                    { "high", new GrowthData { MaturityMonths = 12, HarvestOffsetMonths = 6 } },
                    { "moderate", new GrowthData { MaturityMonths = 18, HarvestOffsetMonths = 6 } },
                    { "low", new GrowthData { MaturityMonths = 24, HarvestOffsetMonths = 6 } },
                }
            },
            { "flower", new()
                {
                    { "high", new GrowthData { MaturityMonths = 1, HarvestOffsetMonths = 0 } },
                    { "moderate", new GrowthData { MaturityMonths = 2, HarvestOffsetMonths = 0 } },
                    { "low", new GrowthData { MaturityMonths = 3, HarvestOffsetMonths = 0 } },
                }
            },
            { "herb", new()
                {
                    { "high", new GrowthData { MaturityMonths = 1, HarvestOffsetMonths = 0 } },
                    { "moderate", new GrowthData { MaturityMonths = 2, HarvestOffsetMonths = 0 } },
                    { "low", new GrowthData { MaturityMonths = 3, HarvestOffsetMonths = 0 } },
                }
            },
            { "vegetable", new()
                {
                    { "high", new GrowthData { MaturityMonths = 1, HarvestOffsetMonths = 0 } },
                    { "moderate", new GrowthData { MaturityMonths = 2, HarvestOffsetMonths = 0 } },
                    { "low", new GrowthData { MaturityMonths = 3, HarvestOffsetMonths = 0 } },
                }
            }
        };

        public static int GetTotalGrowthMonths(string plantType, string growthRate)
        {
            plantType = plantType.ToLower();
            growthRate = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(growthRate.Trim().ToLower());

            if (GrowthTable.TryGetValue(plantType, out var rateMap) && rateMap.TryGetValue(growthRate, out int months))
                return months;

            return 6;
        }

        public static List<int> ParseHarvestSeason(string harvestSeason)
        {
            var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            var monthsMap = monthNames
                .Select((name, index) => new { Name = name.ToLower(), Month = index + 1 })
                .Where(m => !string.IsNullOrEmpty(m.Name))
                .ToDictionary(m => m.Name, m => m.Month);

            return harvestSeason
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(m => m.Trim().ToLower())
                .Where(monthsMap.ContainsKey)
                .Select(m => monthsMap[m])
                .ToList();
        }

        private static int ParseMonthToInt(string monthName)
        {
            var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int i = 0; i < 12; i++)
            {
                if (monthNames[i].Equals(monthName, StringComparison.InvariantCultureIgnoreCase))
                    return i + 1;
            }
            return 0;
        }

        public static List<int> GetIdealPlantingMonths(PlantInfo plant)
        {
            var idealMonths = new List<int>();
            var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            if (!string.IsNullOrWhiteSpace(plant.HarvestSeason))
            {
                var harvestMonths = ParseHarvestSeason(plant.HarvestSeason);
                int growthMonths = GetTotalGrowthMonths(plant.PlantType, plant.GrowthRate);

                foreach (var harvestMonth in harvestMonths)
                {
                    int plantingMonth = harvestMonth - growthMonths;
                    if (plantingMonth <= 0)
                        plantingMonth += 12;

                    idealMonths.Add(plantingMonth);
                }

                return idealMonths.Distinct().OrderBy(m => m).ToList();
            }

            if (plant.PruningMonth != null && plant.PruningMonth.Any())
            {
                var pruningMonths = plant.PruningMonth
                    .Select(pm => ParseMonthToInt(pm))
                    .Where(m => m != 0)
                    .ToList();

                if (pruningMonths.Any())
                {
                    var adjustedMonths = pruningMonths
                        .Select(m => m == 1 ? 12 : m - 1)
                        .Union(pruningMonths)
                        .Distinct()
                        .OrderBy(m => m)
                        .ToList();

                    return adjustedMonths;
                }
            }

            return new List<int> { 3, 4, 5 };
        }

        public static bool IsIdealPlantingTime(PlantInfo plant)
        {
            var idealMonths = GetIdealPlantingMonths(plant);
            int currentMonth = DateTime.UtcNow.Month;
            return idealMonths.Contains(currentMonth);
        }

        public static DateTime GetNextHarvestDate(DateTime plantingDate, string plantType, string growthRate, bool isRecurring, DateTime? lastHarvestDate = null)
        {
            var totalMonths = GetTotalHarvestMonths(plantType, growthRate);

            if (isRecurring && lastHarvestDate.HasValue)
            {
                var offset = GetHarvestOffsetMonths(plantType, growthRate);
                return lastHarvestDate.Value.AddMonths(offset);
            }

            return plantingDate.AddMonths(totalMonths);
        }

        public static int GetHarvestOffsetMonths(string plantType, string growthRate)
        {
            plantType = plantType.Trim().ToLower();
            growthRate = growthRate.Trim().ToLower();

            if (HarvestTable.TryGetValue(plantType, out var rateMap)
                && rateMap.TryGetValue(growthRate, out var data))
            {
                return data.HarvestOffsetMonths;
            }

            return 0;
        }

        public static int GetTotalHarvestMonths(string plantType, string growthRate)
        {
            plantType = plantType.Trim().ToLower();
            growthRate = growthRate.Trim().ToLower();

            if (HarvestTable.TryGetValue(plantType, out var rateMap)
                && rateMap.TryGetValue(growthRate, out var data))
            {
                return data.TotalHarvestMonths;
            }

            return 3;
        }

        public static (bool canHarvest, TimeSpan timeRemaining) CanHarvest(DateTime plantingDate, string plantType, string growthRate, bool isRecurring, DateTime? lastHarvestDate = null)
        {
            var nextHarvestDate = GetNextHarvestDate(plantingDate, plantType, growthRate, isRecurring, lastHarvestDate);
            var now = DateTime.UtcNow;

            if (now >= nextHarvestDate)
                return (true, TimeSpan.Zero);

            var timeRemaining = nextHarvestDate - now;
            return (false, timeRemaining);
        }

        public static bool HasRecurringHarvest(string plantType)
        {
            plantType = plantType.Trim().ToLower();

            if (HarvestTable.TryGetValue(plantType, out var rateMap))
            {
                var sample = rateMap.Values.FirstOrDefault();
                if (sample != null)
                    return sample.HarvestOffsetMonths > 0;
            }

            return false;
        }

        public static (bool canWater, TimeSpan timeUntilNextWater) CanWater(PlantationPlants plantationPlant)
        {
            var plantInfo = plantationPlant.ReferencePlant;
            var lastWatered = plantationPlant.LastWatered;

            if (plantInfo == null || string.IsNullOrWhiteSpace(plantInfo.PlantType) || string.IsNullOrWhiteSpace(plantInfo.Watering))
                return (true, TimeSpan.Zero);

            string plantType = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(plantInfo.PlantType.Trim().ToLower());
            string wateringNeeds = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(plantInfo.Watering.Trim().ToLower());

            if (!WateringTable.TryGetValue(plantType, out var needsMap) || !needsMap.TryGetValue(wateringNeeds, out int intervalHours))
                return (true, TimeSpan.Zero);

            if (!lastWatered.HasValue)
                return (true, TimeSpan.Zero);

            DateTime nextWaterTime = lastWatered.Value.AddHours(intervalHours);
            DateTime now = DateTime.UtcNow;

            if (now >= nextWaterTime)
                return (true, TimeSpan.Zero);

            return (false, nextWaterTime - now);
        }


    }

    public class GrowthData
    {
        public int MaturityMonths { get; set; }
        public int HarvestOffsetMonths { get; set; }
        public int TotalHarvestMonths => MaturityMonths + HarvestOffsetMonths;
    }
}
