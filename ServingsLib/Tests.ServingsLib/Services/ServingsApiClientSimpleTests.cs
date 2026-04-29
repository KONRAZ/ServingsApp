using ServingsLib.Services;

namespace Tests.ServingsLib.Services;

public class ServingsApiClientSimpleTests
{
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
    public async Task GetMenuAsync_ThrowsServingsApiException_WithInvalidUrl()
    {
        // Arrange
        using var client = new ServingsApiClient("http://invalid-url-that-does-not-exist.com", "user", "pass");

        // Act & Assert
        await Assert.ThrowsAsync<ServingsApiException>(() => client.GetMenuAsync());
    }

    [Fact]
    public async Task SendOrderAsync_ThrowsException_WhenOrderIsNull()
    {
        // Arrange
        using var client = new ServingsApiClient("https://test.com", "user", "pass");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.SendOrderAsync(null!));
    }

    [Fact]
    public async Task SendOrderAsync_ThrowsException_WhenOrderIdIsEmpty()
    {
        // Arrange
        var order = new Order("", new List<OrderItem>());
        using var client = new ServingsApiClient("https://test.com", "user", "pass");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => client.SendOrderAsync(order));
        Assert.Contains("OrderId is required", exception.Message);
    }

    [Fact]
    public async Task SendOrderAsync_ThrowsException_WhenMenuItemsIsEmpty()
    {
        // Arrange
        var order = new Order("test-order", new List<OrderItem>());
        using var client = new ServingsApiClient("https://test.com", "user", "pass");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => client.SendOrderAsync(order));
        Assert.Contains("at least one menu item", exception.Message);
    }

    [Fact]
    public async Task SendOrderAsync_ThrowsServingsApiException_WithInvalidUrl()
    {
        // Arrange
        var order = new Order("test-order", new List<OrderItem>
        {
            new OrderItem { Id = "5979224", Quantity = "1" }
        });
        using var client = new ServingsApiClient("http://invalid-url-that-does-not-exist.com", "user", "pass");

        // Act & Assert
        await Assert.ThrowsAsync<ServingsApiException>(() => client.SendOrderAsync(order));
    }

    [Fact]
    public void ServingsApiClient_ImplementsIDisposable()
    {
        // Arrange & Act
        using var client = new ServingsApiClient("https://test.com", "user", "pass");

        // Assert
        Assert.True(client is IDisposable);
    }
}
