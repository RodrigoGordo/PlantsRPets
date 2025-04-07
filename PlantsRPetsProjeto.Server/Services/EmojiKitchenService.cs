using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace PlantsRPetsProjeto.Server.Services
{
    /// <summary>
    /// Serviço responsável por gerar imagens de mascotes a partir de combinações de dois emojis,
    /// utilizando a API do projeto EmojiKitchen.
    /// Verifica também se a imagem gerada corresponde a uma imagem de referência (indicando erro ou ausência de combinação válida).
    /// </summary>
    public class EmojiKitchenService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Caminho para a imagem local de referência, utilizada para comparar e evitar imagens inválidas.
        /// </summary>
        private readonly string localImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "reference.png");

        /// <summary>
        /// Inicializa uma nova instância do <see cref="EmojiKitchenService"/>.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP utilizado para realizar pedidos à API externa.</param>
        public EmojiKitchenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gera a imagem de um mascote com base na combinação de dois emojis fornecidos.
        /// Caso a imagem gerada coincida com a imagem de referência local, considera-se inválida.
        /// </summary>
        /// <param name="emoji1">Primeiro emoji a combinar.</param>
        /// <param name="emoji2">Segundo emoji a combinar.</param>
        /// <param name="size">Tamanho (resolução) da imagem gerada. Valor por omissão: 256.</param>
        /// <returns>URL da imagem gerada, ou string vazia se for inválida ou ocorrer erro.</returns>
        public async Task<string> GeneratePetImageAsync(string emoji1, string emoji2, int size = 256)
        {
            string url = $"https://emojik.vercel.app/s/{emoji1}_{emoji2}?size={size}";

            using Image<Rgba32> onlineImage = await DownloadImage(url);
            using Image<Rgba32> localImage = Image.Load<Rgba32>(localImagePath);

            if (CompareImages(onlineImage, localImage))
            {
                Console.WriteLine("⚠️ Image not found, defaulting to empty.");
                url = "";
            }
           
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

        /// <summary>
        /// Descarrega uma imagem a partir de um URL e carrega-a na memória.
        /// </summary>
        /// <param name="url">URL da imagem a descarregar.</param>
        /// <returns>Instância de <see cref="Image{Rgba32}"/> com a imagem carregada.</returns>
        static async Task<Image<Rgba32>> DownloadImage(string url)
        {
            using HttpClient client = new HttpClient();
            byte[] imageBytes = await client.GetByteArrayAsync(url);
            using MemoryStream ms = new MemoryStream(imageBytes);
            return Image.Load<Rgba32>(ms);
        }

        /// <summary>
        /// Compara duas imagens pixel a pixel.
        /// </summary>
        /// <param name="img1">Primeira imagem.</param>
        /// <param name="img2">Segunda imagem.</param>
        /// <returns>Verdadeiro se forem idênticas em tamanho e conteúdo, falso caso contrário.</returns>
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
