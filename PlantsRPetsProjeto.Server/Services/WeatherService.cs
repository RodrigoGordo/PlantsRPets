using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace PlantsRPetsProjeto.Server.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "1e1bfcd7c4e5498791d120909252602";

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<object> GetWeatherAsync(string city)
        {
            var response = await _httpClient.GetAsync($"http://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}&aqi=no");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var weatherData = await response.Content.ReadFromJsonAsync<object>();
            return weatherData; // Adjust the return type based on your needs
        }



    }
}
