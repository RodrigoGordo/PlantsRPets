using PlantsRPetsProjeto.Server.Models;
using System.Globalization;

namespace PlantsRPetsProjeto.Server.Services
{
    /// <summary>
    /// Serviço utilitário que fornece lógica e cálculos relacionados com o crescimento e colheita de plantas.
    /// Inclui funcionalidades como previsão de datas ideais de plantação, cálculo de ciclos de colheita e identificação de colheitas recorrentes.
    /// </summary>
    public class PlantingAdvisor
    {
        /// <summary>
        /// Tabela de tempos de crescimento em meses, organizada por tipo de planta e ritmo de crescimento.
        /// </summary>
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

        /// <summary>
        /// Tabela com dados de maturação e intervalos de colheita por tipo de planta e ritmo de crescimento.
        /// </summary>
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

        /// <summary>
        /// Devolve o tempo total estimado de crescimento para uma determinada planta.
        /// </summary>
        public static int GetTotalGrowthMonths(string plantType, string growthRate)
        {
            plantType = plantType.ToLower();
            growthRate = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(growthRate.Trim().ToLower());

            if (GrowthTable.TryGetValue(plantType, out var rateMap) && rateMap.TryGetValue(growthRate, out int months))
                return months;

            return 6;
        }

        /// <summary>
        /// Converte uma string de meses (ex: "Março, Abril") para uma lista de inteiros representando os meses.
        /// </summary>
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

        /// <summary>
        /// Converte um nome de mês (ex: "Janeiro") para o respetivo número (1-12).
        /// </summary>
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

        /// <summary>
        /// Calcula os meses ideais de plantação com base na época de colheita ou na época de poda.
        /// </summary>
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

        /// <summary>
        /// Verifica se o mês atual é um mês ideal para plantar a planta indicada.
        /// </summary>
        public static bool IsIdealPlantingTime(PlantInfo plant)
        {
            var idealMonths = GetIdealPlantingMonths(plant);
            int currentMonth = DateTime.UtcNow.Month;
            return idealMonths.Contains(currentMonth);
        }

        /// <summary>
        /// Calcula a data da próxima colheita, considerando se é uma colheita recorrente.
        /// </summary>
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

        /// <summary>
        /// Obtém o número de meses entre colheitas para plantas com colheita recorrente.
        /// </summary>
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

        /// <summary>
        /// Obtém o tempo total (em meses) até à primeira colheita da planta.
        /// </summary>
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

        /// <summary>
        /// Verifica se a planta pode ser colhida no momento atual e, se não puder, devolve o tempo restante.
        /// </summary>
        public static (bool canHarvest, TimeSpan timeRemaining) CanHarvest(DateTime plantingDate, string plantType, string growthRate, bool isRecurring, DateTime? lastHarvestDate = null)
        {
            var nextHarvestDate = GetNextHarvestDate(plantingDate, plantType, growthRate, isRecurring, lastHarvestDate);
            var now = DateTime.UtcNow;

            if (now >= nextHarvestDate)
                return (true, TimeSpan.Zero);

            var timeRemaining = nextHarvestDate - now;
            return (false, timeRemaining);
        }

        /// <summary>
        /// Indica se a planta tem colheitas recorrentes, com base nos dados da tabela de crescimento.
        /// </summary>
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
    }

    /// <summary>
    /// Estrutura auxiliar que representa os dados de maturação e intervalo de colheita de uma planta.
    /// </summary>
    public class GrowthData
    {
        public int MaturityMonths { get; set; }
        public int HarvestOffsetMonths { get; set; }
        public int TotalHarvestMonths => MaturityMonths + HarvestOffsetMonths;
    }
}
