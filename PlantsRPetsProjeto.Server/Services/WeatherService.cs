using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Services
{
    /// <summary>
    /// Serviço responsável por obter dados meteorológicos de uma localização específica
    /// através da API externa WeatherAPI.
    /// </summary>
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "1e1bfcd7c4e5498791d120909252602";

        /// <summary>
        /// Inicializa uma nova instância do <see cref="WeatherService"/> com um cliente HTTP configurado.
        /// </summary>
        /// <param name="httpClient">Instância de <see cref="HttpClient"/> utilizada para fazer os pedidos à API externa.</param>
        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Obtém os dados meteorológicos para uma dada localização (nome de cidade ou coordenadas).
        /// A previsão devolvida inclui três dias, sem índice de qualidade do ar nem alertas.
        /// </summary>
        /// <param name="location">Localização a consultar (ex: nome da cidade ou coordenadas no formato "lat,lon").</param>
        /// <returns>Objeto com os dados meteorológicos ou null caso a resposta não seja bem-sucedida.</returns>
        public async Task<object> GetWeatherAsync(string location)
        {
            var response = await _httpClient.GetAsync($"http://api.weatherapi.com/v1/forecast.json?key={_apiKey}&q={location}&days=3&aqi=no&alerts=no");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var weatherData = await response.Content.ReadFromJsonAsync<object>();
            return weatherData;
        }
    }
}
