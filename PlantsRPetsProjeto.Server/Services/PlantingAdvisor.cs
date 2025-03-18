using System.Globalization;

namespace PlantsRPetsProjeto.Server.Services
{
    public class PlantingAdvisor
    {
        // Tabela para tempos de crescimento (em meses) por tipo e growthRate
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

        // Obtém o tempo total de crescimento para uma planta com base no tipo e growthRate
        public static int GetTotalGrowthMonths(string plantType, string growthRate)
        {
            plantType = plantType.ToLower();
            growthRate = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(growthRate.Trim().ToLower());

            if (GrowthTable.TryGetValue(plantType, out var rateMap) && rateMap.TryGetValue(growthRate, out int months))
                return months;

            // Valor padrão caso não encontrado
            return 6;
        }

        // Calcula os meses ideais de plantação baseado nos meses de colheita e tempo de crescimento
        public static List<int> CalculateIdealPlantingMonths(List<int> harvestMonths, int growthDurationMonths)
        {
            var idealMonths = new List<int>();

            foreach (var harvestMonth in harvestMonths)
            {
                int plantingMonth = harvestMonth - growthDurationMonths;
                if (plantingMonth <= 0)
                    plantingMonth += 12; // Ajuste para ciclo anual

                idealMonths.Add(plantingMonth);
            }

            return idealMonths.Distinct().OrderBy(m => m).ToList();
        }

        // Converte os nomes dos meses da string harvestSeason para inteiros (1 a 12)
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
    }
}
