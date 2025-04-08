using Moq.Protected;
using Moq;
using PlantsRPetsProjeto.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlantsRPetsProjeto.Tests.xUnitTests.ServiceTests
{
    public class CityServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly CityService _cityService;

        public CityServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _cityService = new CityService(_httpClient);
        }

        [Fact]
        public async Task GetCitiesWithNameAsync_ReturnsData_WhenApiResponseIsSuccessful()
        {
            var expectedJson = "[{ \"name\": \"Setúbal\", \"country\": \"Portugal\" }]";
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expectedJson)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponse);

            var result = await _cityService.GetCitiesWithNameAsync("Setubal");

            Assert.NotNull(result);
            var json = (JsonElement)result;
            Assert.Equal(JsonValueKind.Array, json.ValueKind);
            Assert.True(json.GetArrayLength() > 0);
        }

        [Fact]
        public async Task GetCitiesWithNameAsync_ReturnsNull_WhenApiResponseIsUnsuccessful()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponse);

            var result = await _cityService.GetCitiesWithNameAsync("UnknownCity");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetCitiesWithNameAsync_HandlesHttpRequestException()
        {
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            await Assert.ThrowsAsync<HttpRequestException>(() => _cityService.GetCitiesWithNameAsync("Setubal"));
        }

        [Fact]
        public async Task GetCitiesWithNameAsync_ResponseContainsExpectedFields()
        {
            var jsonResponse = @"
            [
                {
                    ""id"": 12345,
                    ""name"": ""Setúbal"",
                    ""region"": ""Setúbal"",
                    ""country"": ""Portugal"",
                    ""lat"": 38.52,
                    ""lon"": -8.89
                }
            ]";

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
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
                .ReturnsAsync(httpResponse);

            var result = await _cityService.GetCitiesWithNameAsync("Setubal");

            Assert.NotNull(result);

            var citiesArray = (JsonElement)result;
            Assert.Equal(JsonValueKind.Array, citiesArray.ValueKind);
            Assert.True(citiesArray.GetArrayLength() > 0);

            var city = citiesArray[0];
            Assert.True(city.TryGetProperty("name", out var name));
            Assert.Equal("Setúbal", name.GetString());

            Assert.True(city.TryGetProperty("country", out var country));
            Assert.Equal("Portugal", country.GetString());

            Assert.True(city.TryGetProperty("lat", out var lat));
            Assert.True(city.TryGetProperty("lon", out var lon));
        }
    }

}

