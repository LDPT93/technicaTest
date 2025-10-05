using ConditionalAPIClient.Models;
using ConditionalAPIClient.Service;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

namespace ConditionalAPIClientTest.ApiClientTests
{
    public class ApiClientTests
    {
        private readonly APIConfig _apiConfig;

        public ApiClientTests()
        {
            _apiConfig = new APIConfig
            {
                BaseUrl = "https://fake-json-api.mock.beeceptor.com",
                APIKey = "reeEQitM0rEsVOdhd7Ed",
                Endpoint1 = "/users",
                Endpoint2 = "/companies",
            };
        }

        [Fact]
        public async Task GetSchedule_ValidFirstEndpoint_ReturnsResponse()
        {
            // Arrange
            var expectedResponse = @"<don_best_sports>
                                                <title>Don Best Schedule</title>
                                                <date>20240911</date>
                                                <link>/users</link>
                                                <id>users</id>
                                                <updated>2024-09-11T18:44:57+0</updated>
                                                <schedule>
                                                </schedule>
                                                </don_best_sports>";
            var apiClient = MockHttpClientEndpoints(expectedResponse);
            var id = $"<id>{_apiConfig.Endpoint1.Split('/').Last()}</id>";

            // Act
            var result = await apiClient.GetSchedule(_apiConfig.Endpoint1);

            // Assert
            Assert.Equal(expectedResponse, result);
            Assert.Contains(id, result);
        }

        [Fact]
        public async Task GetSchedule_ValidSecondEndpoint_ReturnsResponse()
        {
            // Arrange
            var expectedResponse = @"<don_best_sports>
                                                <title>Don Best Schedule</title>
                                                <date>20240911</date>
                                                <link>/companies</link>
                                                <id>companies</id>
                                                <updated>2024-09-11T18:44:57+0</updated>
                                                <schedule>
                                                </schedule>
                                                </don_best_sports>";
            var apiClient = MockHttpClientEndpoints(expectedResponse);
            var id = $"<id>{_apiConfig.Endpoint2.Split('/').Last()}</id>";

            // Act
            var result = await apiClient.GetSchedule(_apiConfig.Endpoint2);

            // Assert
            Assert.Equal(expectedResponse, result);
            Assert.Contains(id, result);
        }

        private ApiClient MockHttpClientEndpoints(string responseContent)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            };
            var mockHandler = new Mock<DelegatingHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient(mockHandler.Object));

            var mockGeneralSettings = new Mock<IOptions<APIConfig>>();
            mockGeneralSettings.Setup(ap => ap.Value).Returns(_apiConfig);

            return new ApiClient(mockHttpClientFactory.Object, mockGeneralSettings.Object);
        }

        [Fact]
        public async Task GetSchedule_EmptyResponse_ReturnsEmptyString()
        {
            // Arrange
            var apiClient = MockHttpClientEndpoints(string.Empty);

            // Act
            var result = await apiClient.GetSchedule(_apiConfig.Endpoint1);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task GetSchedule_HttpError_ThrowsException()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Not Found")
            };
            var mockHandler = new Mock<DelegatingHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient(mockHandler.Object));

            var mockGeneralSettings = new Mock<IOptions<APIConfig>>();
            mockGeneralSettings.Setup(ap => ap.Value).Returns(_apiConfig);

            var apiClient = new ApiClient(mockHttpClientFactory.Object, mockGeneralSettings.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => apiClient.GetSchedule(_apiConfig.Endpoint1));
        }

        [Fact]
        public async Task GetSchedule_InvalidEndpoint_ReturnsEmptyOrError()
        {
            // Arrange
            var apiClient = MockHttpClientEndpoints(string.Empty);
            var invalidEndpoint = "/v2/invalid_endpoint";

            // Act
            var result = await apiClient.GetSchedule(invalidEndpoint);

            // Assert
            Assert.True(result == string.Empty || result == null);
        }

        [Fact]
        public async Task GetSchedule_NetworkException_ThrowsException()
        {
            // Arrange
            var mockHandler = new Mock<DelegatingHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new TaskCanceledException("Request timed out"));

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient(mockHandler.Object));

            var mockGeneralSettings = new Mock<IOptions<APIConfig>>();
            mockGeneralSettings.Setup(ap => ap.Value).Returns(_apiConfig);

            var apiClient = new ApiClient(mockHttpClientFactory.Object, mockGeneralSettings.Object);

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => apiClient.GetSchedule(_apiConfig.Endpoint1));
        }
    }
}
