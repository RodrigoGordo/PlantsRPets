using System;
using System.Threading.Tasks;
using System.Net.Http;
using NBomber.CSharp;

namespace MyLoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using var httpClient = new HttpClient();

            var scenario = Scenario.Create("hello_world_scenario", async context =>
            {
                var response = await httpClient.GetAsync("https://plantsrpets.azurewebsites.net/api/signup");

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10,
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(30))
            );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}