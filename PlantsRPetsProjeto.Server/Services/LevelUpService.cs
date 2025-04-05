using PlantsRPetsProjeto.Server.Data;
using System;
using System.ComponentModel;
using System.Globalization;

namespace PlantsRPetsProjeto.Server.Services
{
    /// <summary>
    /// Serviço responsável por calcular os pontos de experiência atribuídos a ações como rega, plantação e colheita.
    /// Utiliza tabelas internas para atribuir valores com base no tipo de planta, ritmo de crescimento e frequência de rega.
    /// </summary>
    public class LevelUpService
    {
        /// <summary>
        /// Tabela de experiência obtida por maturação e colheita, organizada por tipo de planta e ritmo de crescimento.
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, ExperienceData>> HarvestExperience = new()
        {
            { "tree", new()
                {
                    { "high", new ExperienceData { MaturityExperience = 180, HarvestExperience = 120 } },
                    { "moderate", new ExperienceData { MaturityExperience = 240, HarvestExperience = 120 } },
                    { "low", new ExperienceData { MaturityExperience = 360, HarvestExperience = 120 } },
                }
            },
            { "shrub", new()
                {
                    { "high", new ExperienceData { MaturityExperience = 120, HarvestExperience = 60 } },
                    { "moderate", new ExperienceData { MaturityExperience = 180, HarvestExperience = 60 } },
                    { "low", new ExperienceData { MaturityExperience = 240, HarvestExperience = 60 } },
                }
            },
            { "vine", new()
                {
                    { "high", new ExperienceData { MaturityExperience = 120, HarvestExperience = 60 } },
                    { "moderate", new ExperienceData { MaturityExperience = 180, HarvestExperience = 60 } },
                    { "low", new ExperienceData { MaturityExperience = 240, HarvestExperience = 60 } },
                }
            },
            { "flower", new()
                {
                    { "high", new ExperienceData { MaturityExperience = 10, HarvestExperience = 0 } },
                    { "moderate", new ExperienceData { MaturityExperience = 20, HarvestExperience = 0 } },
                    { "low", new ExperienceData { MaturityExperience = 30, HarvestExperience = 0 } },
                }
            },
            { "herb", new()
                {
                    { "high", new ExperienceData { MaturityExperience = 10, HarvestExperience = 0 } },
                    { "moderate", new ExperienceData { MaturityExperience = 20, HarvestExperience = 0 } },
                    { "low", new ExperienceData { MaturityExperience = 30, HarvestExperience = 0 } },
                }
            },
            { "vegetable", new()
                {
                    { "high", new ExperienceData { MaturityExperience = 10, HarvestExperience = 0 } },
                    { "moderate", new ExperienceData { MaturityExperience = 20, HarvestExperience = 0 } },
                    { "low", new ExperienceData { MaturityExperience = 30, HarvestExperience = 0 } },
                }
            }
        };

        /// <summary>
        /// Tabela de experiência atribuída com base na frequência de rega, por tipo de planta.
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, int>> WateringExperience = new()
        {
            { "tree", new()
                {
                    { "frequent", 3},
                    { "average",  4},
                    { "minimal",  7},
                }
            },
            { "shrub", new()
                {
                    { "frequent",  2},
                    { "average",  4},
                    { "minimal",  6},
                }
            },
            { "vine", new()
                {
                    { "frequent", 2},
                    { "average",  3},
                    { "minimal",  4},
                }
            },
            { "flower", new()
                {
                    { "frequent", 2},
                    { "average",  3},
                    { "minimal",  4},
                }
            },
            { "herb", new()
                {
                    { "frequent", 2},
                    { "average",  2},
                    { "minimal", 1},
                }
            },
            { "vegetable", new()
                {
                    { "frequent",  2},
                    { "average",  2},
                    { "minimal",  1},
                }
            }
        };
           
        


        private readonly PlantsRPetsProjetoServerContext _context;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Construtor do serviço de cálculo de experiência.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP (reservado para possíveis chamadas externas futuras).</param>
        /// <param name="context">Contexto da base de dados da aplicação.</param>
        public LevelUpService(HttpClient httpClient, PlantsRPetsProjetoServerContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        /// <summary>
        /// Calcula a experiência total ou parcial obtida numa colheita, dependendo se é uma colheita recorrente ou não.
        /// </summary>
        /// <param name="plantType">Tipo de planta (ex: tree, flower, etc.).</param>
        /// <param name="growthRate">Velocidade de crescimento (ex: high, moderate, low).</param>
        /// <param name="isRecurring">Indica se a colheita é recorrente.</param>
        /// <param name="lastHarvest">Data da última colheita, se aplicável.</param>
        /// <returns>Valor total de experiência a atribuir.</returns>
        public static int GetHarvestExperienceAmount(string plantType, string growthRate, bool isRecurring, DateTime? lastHarvest = null)
        {
            if (lastHarvest.HasValue && isRecurring) {
                return GetHarvestExperience(plantType, growthRate);
            } else
            {
                return GetTotalExperience(plantType, growthRate);
            }
        }

        /// <summary>
        /// Obtém a experiência total (maturação + colheita) de uma planta que atinge o seu ciclo completo.
        /// </summary>
        public static int GetTotalExperience(string plantType, string growthRate)
        {
            plantType = plantType.ToLower();
            growthRate = growthRate.ToLower();

            if (HarvestExperience.TryGetValue(plantType, out var rateMap) && rateMap.TryGetValue(growthRate, out var data))
                return data.TotalExperience;

            return -1;
        }

        /// <summary>
        /// Obtém apenas a experiência associada à colheita recorrente.
        /// </summary>
        public static int GetHarvestExperience(string plantType, string growthRate)
        {
            plantType = plantType.ToLower();
            growthRate = growthRate.ToLower();

            if (HarvestExperience.TryGetValue(plantType, out var rateMap) && rateMap.TryGetValue(growthRate, out var data))
                return data.HarvestExperience;

            return -1;
        }

        /// <summary>
        /// Obtém a experiência associada à rega, com base no tipo de planta e frequência.
        /// </summary>
        public static int GetWateringExperience(string plantType, string wateringFrequency)
        {
            plantType = plantType.ToLower();
            wateringFrequency = wateringFrequency.ToLower();

            if (WateringExperience.TryGetValue(plantType, out var rateMap) && rateMap.TryGetValue(wateringFrequency, out int experienceAmount))
                return experienceAmount;

            return -1;
        }
    }

    /// <summary>
    /// Estrutura que representa os dados de experiência para maturação e colheita de uma planta.
    /// </summary>
    public class ExperienceData
    {
        public int MaturityExperience {  get; set; }
        public int HarvestExperience { get; set; }

        public int TotalExperience => MaturityExperience + HarvestExperience;
    }
}
