using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using PlantsRPetsProjeto.Server.Services;

namespace PlantsRPetsProjeto.Tests
{
    public class WeatherServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly WeatherService _weatherService;

        public WeatherServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _weatherService = new WeatherService(_httpClient);
        }

        [Fact]
        public async Task GetWeatherAsync_ReturnsWeatherData_WhenApiResponseIsSuccessful()
        {
            var location = "Setubal";
            var expectedResponse = new { location = new { name = "Setubal" }, current = new { temp_c = 20.5 } };
            var jsonResponse = JsonSerializer.Serialize(expectedResponse);
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            var result = await _weatherService.GetWeatherAsync(location);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetWeatherAsync_ReturnsNull_WhenApiResponseIsUnsuccessful()
        {
            var location = "InvalidCity";
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            var result = await _weatherService.GetWeatherAsync(location);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetWeatherAsync_HandlesHttpRequestException()
        {
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            await Assert.ThrowsAsync<HttpRequestException>(() => _weatherService.GetWeatherAsync("Setubal"));
        }

        [Fact]
        public async Task GetWeatherAsync_ResponseContainsExpectedStructure()
        {

            var jsonResponse = @"
        {
            ""location"": {
                ""name"": ""Setúbal"",
                ""region"": ""Setúbal"",
                ""country"": ""Portugal"",
                ""lat"": 38.52,
                ""lon"": -8.89
            },
            ""current"": {
                ""temp_c"": 20.5,
                ""temp_f"": 68.9,
                ""humidity"": 60,
                ""wind_kph"": 15.5,
                ""condition"": {
                    ""text"": ""Partly cloudy"",
                    ""icon"": ""//cdn.weatherapi.com/weather/64x64/day/116.png""
                }
            },
            ""forecast"": {
                ""forecastday"": [
                    {
                        ""date"": ""2025-03-03"",
                        ""day"": {
                            ""avgtemp_c"": 18.0,
                            ""avgtemp_f"": 64.4,
                            ""condition"": {
                                ""text"": ""Sunny"",
                                ""icon"": ""//cdn.weatherapi.com/weather/64x64/day/113.png""
                            }
                        }
                    }
                ]
            }
        }";

            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(fakeResponse);

            // Act: Chama o serviço
            var result = await _weatherService.GetWeatherAsync("Setúbal");

            // Assert: Garante que a resposta não é nula e contém os campos esperados
            Assert.NotNull(result);

            var jsonElement = (JsonElement)result;

            Assert.True(jsonElement.TryGetProperty("location", out var location));
            Assert.True(jsonElement.TryGetProperty("current", out var current));
            Assert.True(jsonElement.TryGetProperty("forecast", out var forecast));

            // Valida que location contém os campos esperados
            Assert.True(location.TryGetProperty("name", out var locationName));
            Assert.Equal("Setúbal", locationName.GetString());

            // Valida que current contém os campos esperados
            Assert.True(current.TryGetProperty("temp_c", out var tempC));
            Assert.True(tempC.ValueKind == JsonValueKind.Number);

            Assert.True(current.TryGetProperty("humidity", out var humidity));
            Assert.True(humidity.ValueKind == JsonValueKind.Number);

            Assert.True(current.TryGetProperty("condition", out var condition));
            Assert.True(condition.TryGetProperty("text", out var conditionText));
            Assert.True(conditionText.ValueKind == JsonValueKind.String);

            // Valida que forecast contém pelo menos um dia de previsão
            Assert.True(forecast.TryGetProperty("forecastday", out var forecastDay));
            Assert.Equal(JsonValueKind.Array, forecastDay.ValueKind);
            Assert.True(forecastDay.GetArrayLength() > 0);
        }

    }
}
