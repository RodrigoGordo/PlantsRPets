using System.Net.Http;
using System.Threading.Tasks;


namespace PlantsRPetsProjeto.Server.Services
{
    public class EmojiKitchenService
    {
        private readonly HttpClient _httpClient;

        public EmojiKitchenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GeneratePetImageAsync(string emoji1, string emoji2, int size = 256)
        {
            string url = $"https://emojik.vercel.app/s/{emoji1}_{emoji2}?size={size}";

            // Call the API and return the image URL
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return url;
        }


    }
}
