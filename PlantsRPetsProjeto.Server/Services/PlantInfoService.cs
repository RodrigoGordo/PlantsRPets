using PlantsRPetsProjeto.Server.Models;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlantsRPetsProjeto.Server.Services
{
    public class PlantInfoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "sk-uyOs67c9be8fd5f0c8989";
        public PlantInfoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PlantInfo>> GetPlantsAsync(int startId, int maxId)
        {
            var plants = new List<PlantInfo>();
            int id = startId;

            while (id <= maxId)
            {
                var url = $"https://perenual.com/api/species/details/{id}?key={_apiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to fetch plant {id}: {response.StatusCode}");
                }
                else
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                    var plant = MapToPlantInfo(jsonData);
                    plants.Add(plant);
                }

                id++;
                await Task.Delay(2000);
            }

            return plants;
        }

        private PlantInfo MapToPlantInfo(JsonElement jsonData)
        {
            return new PlantInfo
            {
                PlantName = jsonData.TryGetProperty("common_name", out var commonName) && commonName.ValueKind != JsonValueKind.Null
                    ? commonName.GetString() ?? "" : "",
                PlantType = jsonData.TryGetProperty("type", out var plantType) && plantType.ValueKind != JsonValueKind.Null
                    ? plantType.GetString() ?? "" : "",
                Cycle = jsonData.TryGetProperty("cycle", out var cycle) && cycle.ValueKind != JsonValueKind.Null
                    ? cycle.GetString() ?? "" : "",
                Watering = jsonData.TryGetProperty("watering", out var watering) && watering.ValueKind != JsonValueKind.Null
                    ? watering.GetString() ?? "" : "",
                PruningMonth = jsonData.TryGetProperty("pruning_month", out var pruningMonth) && pruningMonth.ValueKind == JsonValueKind.Array
                    ? pruningMonth.EnumerateArray().Select(p => p.GetString() ?? "").ToList() : new List<string>(),
                PruningCount = jsonData.TryGetProperty("pruning_count", out var pruningCount) && pruningCount.ValueKind == JsonValueKind.Object
                    ? new PruningCountInfo
                    {
                        Amount = pruningCount.TryGetProperty("amount", out var amount) && amount.ValueKind == JsonValueKind.Number
                            ? amount.GetInt32() : 0,
                        Interval = pruningCount.TryGetProperty("interval", out var interval) && interval.ValueKind != JsonValueKind.Null
                            ? interval.GetString() ?? "" : ""
                    }
                    : null,
                GrowthRate = jsonData.TryGetProperty("growth_rate", out var growthRate) && growthRate.ValueKind != JsonValueKind.Null
                    ? growthRate.GetString() ?? "" : "",
                Sunlight = jsonData.TryGetProperty("sunlight", out var sunlight) && sunlight.ValueKind == JsonValueKind.Array
                    ? sunlight.EnumerateArray().Select(s => s.GetString() ?? "").ToList() : new List<string>(),
                Edible = jsonData.TryGetProperty("edible_fruit", out var edible) && edible.ValueKind == JsonValueKind.True
                    ? "Yes" : "No",
                CareLevel = jsonData.TryGetProperty("care_level", out var careLevel) && careLevel.ValueKind != JsonValueKind.Null
                    ? careLevel.GetString() ?? "" : "",
                Flowers = jsonData.TryGetProperty("flowers", out var flowers) && flowers.ValueKind == JsonValueKind.True
                    ? "Yes" : "No",
                Fruits = jsonData.TryGetProperty("fruits", out var fruits) && fruits.ValueKind == JsonValueKind.True
                    ? "Yes" : "No",
                Leaf = jsonData.TryGetProperty("leaf", out var leaf) && leaf.ValueKind == JsonValueKind.True,
                Maintenance = jsonData.TryGetProperty("maintenance", out var maintenance) && maintenance.ValueKind != JsonValueKind.Null
                    ? maintenance.GetString() ?? "" : "",
                SaltTolerant = jsonData.TryGetProperty("salt_tolerant", out var saltTolerant) && saltTolerant.ValueKind == JsonValueKind.True
                    ? "Yes" : "No",
                Indoor = jsonData.TryGetProperty("indoor", out var indoor) && indoor.ValueKind == JsonValueKind.True,
                FloweringSeason = jsonData.TryGetProperty("flowering_season", out var floweringSeason) && floweringSeason.ValueKind != JsonValueKind.Null
                    ? floweringSeason.GetString() ?? "" : "",
                Description = jsonData.TryGetProperty("description", out var description) && description.ValueKind != JsonValueKind.Null
                    ? description.GetString() ?? "" : "",
                Image = jsonData.TryGetProperty("default_image", out var defaultImage) && defaultImage.TryGetProperty("regular_url", out var imageUrl) && imageUrl.ValueKind != JsonValueKind.Null
                    ? imageUrl.GetString() ?? "" : "",
                HarvestSeason = jsonData.TryGetProperty("harvest_season", out var harvestSeason) && harvestSeason.ValueKind != JsonValueKind.Null
                    ? harvestSeason.GetString() ?? "" : "",
                ScientificName = jsonData.TryGetProperty("scientific_name", out var scientificName) && scientificName.ValueKind == JsonValueKind.Array
                    ? scientificName.EnumerateArray().Select(s => s.GetString() ?? "").ToList() : new List<string>(),
                DroughtTolerant = jsonData.TryGetProperty("drought_tolerant", out var droughtTolerant) && droughtTolerant.ValueKind == JsonValueKind.True,
                Cuisine = jsonData.TryGetProperty("cuisine", out var cuisine) && cuisine.ValueKind == JsonValueKind.True,
                Medicinal = jsonData.TryGetProperty("medicinal", out var medicinal) && medicinal.ValueKind == JsonValueKind.True
            };
        }

    }
}
