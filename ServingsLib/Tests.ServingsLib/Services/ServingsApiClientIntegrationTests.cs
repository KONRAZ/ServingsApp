using System.Net;
using System.Text;
using Moq.Protected;
using ServingsLib.Services;

namespace Tests.ServingsLib.Services;

public class ServingsApiClientIntegrationTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHandler;
    private readonly HttpClient _httpClient;
    private readonly ServingsApiClient _apiClient;

    public ServingsApiClientIntegrationTests()
    {
        _mockHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHandler.Object);
    }

    [Fact]
    public void ServingsApiClient_CanBeCreated_WithValidParameters()
    {
        // Arrange
        var baseUrl = "https://test.com";
        var username = "testuser";
        var password = "testpass";

        // Act
        using var client = new ServingsApiClient(baseUrl, username, password);

        // Assert
        Assert.NotNull(client);
    }

    [Theory]
    [InlineData(null, "user", "pass")]
    [InlineData("", "user", "pass")]
    [InlineData("   ", "user", "pass")]
    [InlineData("https://test.com", null, "pass")]
    [InlineData("https://test.com", "", "pass")]
    [InlineData("https://test.com", "   ", "pass")]
    [InlineData("https://test.com", "user", null)]
    [InlineData("https://test.com", "user", "")]
    [InlineData("https://test.com", "user", "   ")]
    public void ServingsApiClient_ThrowsException_WithInvalidParameters(string baseUrl, string username, string password)
    {
        // Arrange & Act & Assert
        Assert.ThrowsAny<ArgumentException>(() => 
            new ServingsApiClient(baseUrl, username, password));
    }

    [Fact]
    public async Task GetMenuAsync_ReturnsMenuItems_WhenServerReturnsSuccess()
    {
        // Arrange
        var expectedResponse = @"{
            ""Command"": ""GetMenu"",
            ""Success"": true,
            ""ErrorMessage"": """",
            ""Data"": {
                ""MenuItems"": [
                    {
                        ""Id"": ""5979224"",
                        ""Article"": ""A1004292"",
                        ""Name"": ""Каша гречневая"",
                        ""Price"": 50,
                        ""IsWeighted"": false,
                        ""FullPath"": ""ПРОИЗВОДСТВО\\Гарниры"",
                        ""Barcodes"": [""57890975627974236429""]
                    }
                ]
            }
        }";

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
        };

        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        // Создаем клиент с мокированным HttpClient через рефлексию
        var apiClient = CreateClientWithMockedHttpClient(_httpClient);

        // Act
        var result = await apiClient.GetMenuAsync();

        // Assert
        Assert.Single(result);
        var menuItem = result.First();
        Assert.Equal("5979224", menuItem.Id);
        Assert.Equal("A1004292", menuItem.Article);
        Assert.Equal("Каша гречневая", menuItem.Name);
        Assert.Equal(50m, menuItem.Price);
        Assert.False(menuItem.IsWeighted);
        Assert.Equal("ПРОИЗВОДСТВО\\Гарниры", menuItem.FullPath);
        Assert.Single(menuItem.Barcodes);
        Assert.Equal("57890975627974236429", menuItem.Barcodes.First());
    }

    [Fact]
    public async Task GetMenuAsync_ReturnsEmptyList_WhenDataIsNull()
    {
        // Arrange
        var expectedResponse = @"{
            ""Command"": ""GetMenu"",
            ""Success"": true,
            ""ErrorMessage"": """",
            ""Data"": null
        }";

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
        };

        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var apiClient = CreateClientWithMockedHttpClient(_httpClient);

        // Act
        var result = await apiClient.GetMenuAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMenuAsync_ThrowsException_WhenServerReturnsError()
    {
        // Arrange
        var expectedResponse = @"{
            ""Command"": ""GetMenu"",
            ""Success"": false,
            ""ErrorMessage"": ""Server error occurred""
        }";

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
        };

        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var apiClient = CreateClientWithMockedHttpClient(_httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ServingsApiException>(() => apiClient.GetMenuAsync());
        Assert.Contains("Server returned error", exception.Message);
        Assert.Equal("Server error occurred", exception.ErrorMessage);
    }

    [Fact]
    public async Task SendOrderAsync_Succeeds_WhenOrderIsValid()
    {
        // Arrange
        var order = new Order("test-order-id", new List<OrderItem>
        {
            new OrderItem { Id = "5979224", Quantity = "1" }
        });

        var expectedResponse = @"{
            ""Command"": ""SendOrder"",
            ""Success"": true,
            ""ErrorMessage"": """"
        }";

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
        };

        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var apiClient = CreateClientWithMockedHttpClient(_httpClient);

        // Act & Assert
        await apiClient.SendOrderAsync(order);
    }

    [Fact]
    public async Task SendOrderAsync_ThrowsException_WhenOrderIsNull()
    {
        // Arrange
        var apiClient = CreateClientWithMockedHttpClient(_httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => apiClient.SendOrderAsync(null!));
    }

    [Fact]
    public async Task SendOrderAsync_ThrowsException_WhenOrderIdIsEmpty()
    {
        // Arrange
        var order = new Order("", new List<OrderItem>());
        var apiClient = CreateClientWithMockedHttpClient(_httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => apiClient.SendOrderAsync(order));
        Assert.Contains("OrderId is required", exception.Message);
    }

    [Fact]
    public async Task SendOrderAsync_ThrowsException_WhenMenuItemsIsEmpty()
    {
        // Arrange
        var order = new Order("test-order", new List<OrderItem>());
        var apiClient = CreateClientWithMockedHttpClient(_httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => apiClient.SendOrderAsync(order));
        Assert.Contains("at least one menu item", exception.Message);
    }

    [Fact]
    public async Task SendOrderAsync_ThrowsException_WhenServerReturnsError()
    {
        // Arrange
        var order = new Order("test-order", new List<OrderItem>
        {
            new OrderItem { Id = "5979224", Quantity = "1" }
        });

        var expectedResponse = @"{
            ""Command"": ""SendOrder"",
            ""Success"": false,
            ""ErrorMessage"": ""Order processing failed""
        }";

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(expectedResponse, Encoding.UTF8, "application/json")
        };

        _mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var apiClient = CreateClientWithMockedHttpClient(_httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ServingsApiException>(() => apiClient.SendOrderAsync(order));
        Assert.Contains("Server returned error", exception.Message);
        Assert.Equal("Order processing failed", exception.ErrorMessage);
    }

    private ServingsApiClient CreateClientWithMockedHttpClient(HttpClient httpClient)
    {
        // Создаем клиент через рефлексию, чтобы подменить HttpClient
        var constructorInfo = typeof(ServingsApiClient)
            .GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .FirstOrDefault();

        if (constructorInfo == null)
        {
            // Если нет приватного конструктора, создаем через публичный и заменяем поле
            var client = new ServingsApiClient("https://test.com", "user", "pass");
            var httpClientField = typeof(ServingsApiClient)
                .GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            httpClientField?.SetValue(client, httpClient);
            return client;
        }

        return new ServingsApiClient("https://test.com", "user", "pass");
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
