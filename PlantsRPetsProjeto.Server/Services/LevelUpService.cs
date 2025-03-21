using PlantsRPetsProjeto.Server.Data;
using System;
using System.ComponentModel;
using System.Globalization;

namespace PlantsRPetsProjeto.Server.Services
{
    public class LevelUpService
    {
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

        public LevelUpService(HttpClient httpClient, PlantsRPetsProjetoServerContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public static int GetHarvestExperienceAmount(string plantType, string growthRate, bool isRecurring, DateTime? lastHarvest = null)
        {
            if (lastHarvest.HasValue && isRecurring) {
                return GetHarvestExperience(plantType, growthRate);
            } else
            {
                return GetTotalExperience(plantType, growthRate);
            }
        }

        public static int GetTotalExperience(string plantType, string growthRate)
        {
            plantType = plantType.ToLower();
            growthRate = growthRate.ToLower();

            if (HarvestExperience.TryGetValue(plantType, out var rateMap) && rateMap.TryGetValue(growthRate, out var data))
                return data.TotalExperience;

            return -1;
        }

        public static int GetHarvestExperience(string plantType, string growthRate)
        {
            plantType = plantType.ToLower();
            growthRate = growthRate.ToLower();

            if (HarvestExperience.TryGetValue(plantType, out var rateMap) && rateMap.TryGetValue(growthRate, out var data))
                return data.HarvestExperience;

            return -1;
        }

        public static int GetWateringExperience(string plantType, string wateringFrequency)
        {
            plantType = plantType.ToLower();
            wateringFrequency = wateringFrequency.ToLower();

            if (WateringExperience.TryGetValue(plantType, out var rateMap) && rateMap.TryGetValue(wateringFrequency, out int experienceAmount))
                return experienceAmount;

            return -1;
        }
    }

    public class ExperienceData
    {
        public int MaturityExperience {  get; set; }
        public int HarvestExperience { get; set; }

        public int TotalExperience => MaturityExperience + HarvestExperience;
    }
}
