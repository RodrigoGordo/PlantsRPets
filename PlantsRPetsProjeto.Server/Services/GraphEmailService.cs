using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Server.Services
{
    public class GraphEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GraphEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var tenantId = _configuration["Email:TenantId"];
            var clientId = _configuration["Email:ClientId"];
            var clientSecret = _configuration["Email:ClientSecret"];

            var tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default")
            });

            var response = await _httpClient.PostAsync(tokenEndpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            // **DEBUG: Imprimir a resposta da API**
            Console.WriteLine($"[DEBUG] Resposta da Microsoft Graph API: {responseString}");

            using var jsonDoc = JsonDocument.Parse(responseString);

            // **Evita erro caso "access_token" não exista**
            if (!jsonDoc.RootElement.TryGetProperty("access_token", out var tokenElement))
            {
                throw new Exception($"Erro ao obter o token de acesso: {responseString}");
            }

            return tokenElement.GetString();
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var accessToken = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var emailJson = new
            {
                message = new
                {
                    subject = subject,
                    body = new { contentType = "HTML", content = body },
                    toRecipients = new[]
                    {
                new { emailAddress = new { address = to } }
            }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(emailJson), Encoding.UTF8, "application/json");

            // Enviar requisição para a API Graph
            var senderEmail = "202200038@estudantes.ips.pt"; // O email autenticado no Azure AD
            var response = await _httpClient.PostAsync($"https://graph.microsoft.com/v1.0/users/{senderEmail}/sendMail", jsonContent);

            // **Log para Debugging**
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] Resposta da API Graph: {responseString}");

            // Se houver erro, lançar exceção com detalhes
            response.EnsureSuccessStatusCode();
        }
    }
}
