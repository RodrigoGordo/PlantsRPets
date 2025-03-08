using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Services
{
    public class SustainabilityTipService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public SustainabilityTipService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiSettings:PerenualApiKey"] ?? throw new ArgumentNullException("API Key is missing!");
        }

        public async Task<List<SustainabilityTipsList>> GetSustainabilityTipsAsync(int startId, int maxId)
        {
            var tasks = new List<Task<SustainabilityTipsList?>>();
            for (int plantId = startId; plantId <= maxId; plantId++)
            {
                tasks.Add(FetchSustainabilityTipsAsync(plantId));
            }

            var results = await Task.WhenAll(tasks);
            return results.Where(result => result != null).ToList()!;
        }

        private async Task<SustainabilityTipsList?> FetchSustainabilityTipsAsync(int plantId)
        {
            try
            {
                var url = $"https://perenual.com/api/species-care-guide-list?key={_apiKey}&species_id={plantId}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to fetch sustainability tips for plant {plantId}: {response.StatusCode}");
                    return null;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonData = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                return MapToSustainabilityTipsList(jsonData, plantId);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error fetching sustainability tips for plant {plantId}: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON for sustainability tips of plant {plantId}: {ex.Message}");
                return null;
            }
        }

        private SustainabilityTipsList MapToSustainabilityTipsList(JsonElement jsonData, int plantId)
        {
            if (!jsonData.TryGetProperty("data", out var dataArray) || dataArray.ValueKind != JsonValueKind.Array || dataArray.GetArrayLength() == 0)
                return new SustainabilityTipsList { PlantInfoId = plantId, PlantName = "", PlantScientificName = new List<string>(), SustainabilityTip = new List<SustainabilityTip>() };

            var plantData = dataArray[0];

            return new SustainabilityTipsList
            {
                PlantInfoId = plantId,
                PlantName = plantData.TryGetProperty("common_name", out var commonName) && commonName.ValueKind != JsonValueKind.Null
                    ? commonName.GetString() ?? "" : "",
                PlantScientificName = plantData.TryGetProperty("scientific_name", out var scientificName) && scientificName.ValueKind == JsonValueKind.Array
                    ? scientificName.EnumerateArray().Select(s => s.GetString() ?? "").ToList() : new List<string>(),
                SustainabilityTip = plantData.TryGetProperty("section", out var section) && section.ValueKind == JsonValueKind.Array
                    ? section.EnumerateArray().Select(tip => new SustainabilityTip
                    {
                        Type = tip.TryGetProperty("type", out var type) && type.ValueKind != JsonValueKind.Null ? type.GetString() ?? "" : "",
                        Description = tip.TryGetProperty("description", out var description) && description.ValueKind != JsonValueKind.Null ? description.GetString() ?? "" : ""
                    }).ToList()
                    : new List<SustainabilityTip>()
            };
        }
    }
}
