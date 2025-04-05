using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Services
{
    /// <summary>
    /// Serviço responsável por obter dicas de sustentabilidade para plantas
    /// a partir da API externa da Perenual.
    /// </summary>
    public class SustainabilityTipService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="SustainabilityTipService"/>.
        /// </summary>
        /// <param name="httpClient">Instância do cliente HTTP usada para comunicação com a API externa.</param>
        /// <param name="configuration">Fonte de configuração usada para obter a chave de API.</param>
        /// <exception cref="ArgumentNullException">Lançada se a chave de API estiver em falta.</exception>
        public SustainabilityTipService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiSettings:PerenualApiKey"] ?? throw new ArgumentNullException("API Key is missing!");
        }

        /// <summary>
        /// Obtém dicas de sustentabilidade para um intervalo de IDs de plantas.
        /// </summary>
        /// <param name="startId">ID inicial da planta.</param>
        /// <param name="maxId">ID final da planta.</param>
        /// <returns>Lista de objetos <see cref="SustainabilityTipsList"/> com as dicas correspondentes.</returns>
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

        /// <summary>
        /// Obtém dicas de sustentabilidade para uma planta específica.
        /// </summary>
        /// <param name="plantId">Identificador da planta.</param>
        /// <returns>Objeto <see cref="SustainabilityTipsList"/> ou null caso ocorra erro ou a resposta seja inválida.</returns>
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

        /// <summary>
        /// Mapeia a resposta JSON da API para um objeto <see cref="SustainabilityTipsList"/>.
        /// </summary>
        /// <param name="jsonData">Dados em formato JSON devolvidos pela API.</param>
        /// <param name="plantId">ID da planta associada às dicas.</param>
        /// <returns>Objeto <see cref="SustainabilityTipsList"/> com a informação extraída.</returns>
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
