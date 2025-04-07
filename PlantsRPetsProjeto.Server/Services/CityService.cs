namespace PlantsRPetsProjeto.Server.Services
{
    public class CityService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "1e1bfcd7c4e5498791d120909252602";

        public CityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<object> GetCitiesWithNameAsync(string name)
        {
            var response = await _httpClient.GetAsync($"http://api.weatherapi.com/v1/search.json?key={_apiKey}&q={name}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var cities = await response.Content.ReadFromJsonAsync<object>();
            return cities;
        }
    }
}