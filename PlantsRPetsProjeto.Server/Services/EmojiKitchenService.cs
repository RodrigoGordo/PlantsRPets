using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace PlantsRPetsProjeto.Server.Services
{
    public class EmojiKitchenService
    {
        private readonly HttpClient _httpClient;
        private readonly string localImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "reference.png");

        public EmojiKitchenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GeneratePetImageAsync(string emoji1, string emoji2, int size = 256)
        {
            string url = $"https://emojik.vercel.app/s/{emoji1}_{emoji2}?size={size}";

            // Download the image
            using Image<Rgba32> onlineImage = await DownloadImage(url);
            using Image<Rgba32> localImage = Image.Load<Rgba32>(localImagePath);

            // Compare images, and if they are the same, skip the generation
            if (CompareImages(onlineImage, localImage))
            {
                Console.WriteLine("⚠️ Image not found, defaulting to empty.");
                url = "";
            }
           
            // Call the API and return the image URL
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return url;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching new image: {ex.Message}");
                return "";
            }
        }

        static async Task<Image<Rgba32>> DownloadImage(string url)
        {
            using HttpClient client = new HttpClient();
            byte[] imageBytes = await client.GetByteArrayAsync(url);
            using MemoryStream ms = new MemoryStream(imageBytes);
            return Image.Load<Rgba32>(ms);
        }

        private static bool CompareImages(Image<Rgba32> img1, Image<Rgba32> img2)
        {
            if (img1.Width != img2.Width || img1.Height != img2.Height)
                return false;

            for (int y = 0; y < img1.Height; y++)
            {
                for (int x = 0; x < img1.Width; x++)
                {
                    if (img1[x, y] != img2[x, y])
                        return false;
                }
            }
            return true;
        }

    }
}
