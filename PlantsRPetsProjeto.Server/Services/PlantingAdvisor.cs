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
    }
}
