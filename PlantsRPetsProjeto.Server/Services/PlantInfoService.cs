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

        public async Task<PlantInfo> GetPlantsAsync()
        {
            //var plants = new List<PlantInfo>();
            int id = 1;
            //bool hasMoreResults = true;

            //while (hasMoreResults)
            //{
                var url = $"https://perenual.com/api/species/details/{id}?key={_apiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to fetch data: {response.StatusCode}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                Console.WriteLine(jsonResponse);
                var jsonData = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

                var plant = MapToPlantInfo(jsonData);

                //plants.Add(plant);

                //hasMoreResults = false;
            //}

            return plant;
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
                PruningCount = jsonData.TryGetProperty("pruning_count", out var pruning) && pruning.ValueKind == JsonValueKind.Array
                    ? pruning.GetArrayLength().ToString() : "N/A",
                GrowthRate = jsonData.TryGetProperty("growth_rate", out var growthRate) && growthRate.ValueKind != JsonValueKind.Null
                    ? growthRate.GetString() ?? "" : "",
                Sun = jsonData.TryGetProperty("sunlight", out var sunlight) && sunlight.ValueKind == JsonValueKind.Array
                    ? sunlight.EnumerateArray().FirstOrDefault().GetString() ?? "" : "",
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
                SunDuration = jsonData.TryGetProperty("sunlight", out var sunDuration) && sunDuration.ValueKind == JsonValueKind.Array
                    ? string.Join(", ", sunDuration.EnumerateArray().Select(s => s.GetString() ?? "")) : "",
                FloweringSeason = jsonData.TryGetProperty("flowering_season", out var floweringSeason) && floweringSeason.ValueKind != JsonValueKind.Null
                    ? floweringSeason.GetString() ?? "" : "",
                Description = jsonData.TryGetProperty("description", out var description) && description.ValueKind != JsonValueKind.Null
                    ? description.GetString() ?? "" : "",
                Image = jsonData.TryGetProperty("default_image", out var defaultImage) && defaultImage.TryGetProperty("regular_url", out var imageUrl) && imageUrl.ValueKind != JsonValueKind.Null
                    ? imageUrl.GetString() ?? "" : ""
            };
        }

    }
}
