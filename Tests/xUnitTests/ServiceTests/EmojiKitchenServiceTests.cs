using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using PlantsRPetsProjeto.Server.Services;
using Xunit;

namespace PlantsRPetsProjeto.Tests.xUnitTests.EmojiKitchenServiceTests
{
    public class EmojiKitchenServiceTests
    {
        [Fact]
        public async Task GeneratePetImageAsync_ReturnsCorrectUrl()
        {
            var emoji1 = "🍉";
            var emoji2 = "😀";
            var size = 256;
            var expectedUrl = $"https://emojik.vercel.app/s/{emoji1}_{emoji2}?size={size}";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new EmojiKitchenService(httpClient);

            var result = await service.GeneratePetImageAsync(emoji1, emoji2, size);

            Assert.Equal(expectedUrl, result);
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString() == expectedUrl),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GeneratePetImageAsync_WithDefaultSize_ReturnsCorrectUrl()
        {
            var emoji1 = "🌻";
            var emoji2 = "🐶";
            var expectedUrl = $"https://emojik.vercel.app/s/{emoji1}_{emoji2}?size=256";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new EmojiKitchenService(httpClient);

            var result = await service.GeneratePetImageAsync(emoji1, emoji2);

            Assert.Equal(expectedUrl, result);
        }

        [Fact]
        public async Task GeneratePetImageAsync_WhenApiFailsWithNonSuccessStatusCode_ThrowsException()
        {
            var emoji1 = "🍌";
            var emoji2 = "🐱";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new EmojiKitchenService(httpClient);

            await Assert.ThrowsAsync<HttpRequestException>(() =>
                service.GeneratePetImageAsync(emoji1, emoji2));
        }
    }
}