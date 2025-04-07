using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace PlantsRPetsLoadTest
{
  class Program
  {
    static void Main(string[] args)
    {
      using var httpClient = new HttpClient();
      string baseUrl = "https://plantsarepets.azurewebsites.net";

      var scenario = Scenario.Create("RegisterLoginPlantations", async context =>
      {
        var invocationId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
        var email = $"user{invocationId}@test.com";
        var password = "Test123!";
        var registerJson = $@"{{ ""email"": ""{email}"", ""password"": ""{password}"" }}";

        // 1. Register
        var registerRequest = Http.CreateRequest("POST", $"{baseUrl}/auth/register")
                                  .WithHeader("Content-Type", "application/json")
                                  .WithBody(new StringContent(registerJson, Encoding.UTF8, "application/json"));

        var registerResponse = await Http.Send(httpClient, registerRequest);

        if (!registerResponse.IsSuccessStatusCode)
          return Response.Fail("Erro no registro");

        // 2. Login
        var loginJson = $@"{{ ""email"": ""{email}"", ""password"": ""{password}"" }}";

        var loginRequest = Http.CreateRequest("POST", $"{baseUrl}/auth/login")
                               .WithHeader("Content-Type", "application/json")
                               .WithBody(new StringContent(loginJson, Encoding.UTF8, "application/json"));

        var loginResponse = await Http.Send(httpClient, loginRequest);

        if (!loginResponse.IsSuccessStatusCode)
          return Response.Fail("Erro no login");

        var loginContent = await loginResponse.Response.Content.ReadAsStringAsync();
        var token = JObject.Parse(loginContent)["token"]?.ToString();

        if (string.IsNullOrWhiteSpace(token))
          return Response.Fail("Token não encontrado");

        // 3. Criar plantação
        var plantationJson = $@"{{
                    ""name"": ""Minha Plantinha {invocationId}"",
                    ""seedType"": ""Tomate"",
                    ""plantedOn"": ""{DateTime.UtcNow:yyyy-MM-dd}""
                }}";

        var createRequest = Http.CreateRequest("POST", $"{baseUrl}/plantations")
                                .WithHeader("Authorization", $"Bearer {token}")
                                .WithHeader("Content-Type", "application/json")
                                .WithBody(new StringContent(plantationJson, Encoding.UTF8, "application/json"));

        var createResponse = await Http.Send(httpClient, createRequest);

        if (!createResponse.IsSuccessStatusCode)
          return Response.Fail("Erro ao criar plantação");

        // 4. Buscar plantações
        var getRequest = Http.CreateRequest("GET", $"{baseUrl}/plantations")
                             .WithHeader("Authorization", $"Bearer {token}")
                             .WithHeader("Accept", "application/json");

        var getResponse = await Http.Send(httpClient, getRequest);

        return getResponse.IsSuccessStatusCode
            ? Response.Ok()
            : Response.Fail("Erro ao buscar plantações");
      })
      .WithoutWarmUp()
      .WithLoadSimulations(
          Simulation.Inject(rate: 5, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30))
      );

      NBomberRunner
          .RegisterScenarios(scenario)
          .Run();
    }
  }
}
