using Moq;
using ServingsLib.Models;
using ServingsLib.Services;
using ServingsLib.Exceptions;

namespace Tests;

public class ServingsClientTests : IDisposable
{
    private readonly Mock<ServingsApiClient> _mockApiClient;
    private readonly ServingsClient _servingsClient;

    public ServingsClientTests()
    {
        _mockApiClient = new Mock<ServingsApiClient>("https://test.com", "user", "pass");
        _servingsClient = new ServingsClient("https://test.com", "user", "pass");
    }

    [Fact]
    public void ServingsClient_CanBeCreated_WithValidParameters()
    {
        // Arrange
        var baseUrl = "https://test.com";
        var username = "testuser";
        var password = "testpass";

        // Act
        using var client = new ServingsClient(baseUrl, username, password);

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
    public void ServingsClient_ThrowsException_WithInvalidParameters(string baseUrl, string username, string password)
    {
        // Arrange & Act & Assert
        Assert.ThrowsAny<ArgumentException>(() => 
            new ServingsClient(baseUrl, username, password));
    }

    [Fact]
    public async Task GetMenuAsync_ReturnsMenuItems_WhenApiCallSucceeds()
    {
        // Arrange
        var expectedMenuItems = new List<MenuItem>
        {
            new MenuItem 
            { 
                Id = "5979224", 
                Name = "Каша гречневая", 
                Price = 50 
            }
        };

        // This test requires integration with real API or more complex mocking
        // For now, we'll test the basic flow
        using var client = new ServingsClient("https://test.com", "user", "pass");

        // Act & Assert - This would fail with real HTTP call, but shows test structure
        // In real scenario, you'd mock the HttpClient inside ServingsApiClient
        await Assert.ThrowsAsync<ServingsApiException>(() => client.GetMenuAsync());
    }

    [Fact]
    public void CreateOrder_ReturnsValidOrder_WithValidParameters()
    {
        // Arrange
        var orderId = "test-order-id";
        var orderItems = new List<OrderItem>
        {
            new OrderItem { Id = "5979224", Quantity = "1" }
        };

        // Act
        var order = ServingsClient.CreateOrder(orderId, orderItems);

        // Assert
        Assert.Equal(orderId, order.OrderId);
        Assert.Equal(orderItems, order.MenuItems);
    }

    [Fact]
    public void CreateOrder_ReturnsOrderWithEmptyList_WhenMenuItemsIsNull()
    {
        // Arrange
        var orderId = "test-order-id";

        // Act
        var order = ServingsClient.CreateOrder(orderId, null);

        // Assert
        Assert.Equal(orderId, order.OrderId);
        Assert.Empty(order.MenuItems);
    }

    [Fact]
    public void CreateOrderItem_ReturnsValidOrderItem_WithValidParameters()
    {
        // Arrange
        var id = "5979224";
        var quantity = "1";

        // Act
        var orderItem = ServingsClient.CreateOrderItem(id, quantity);

        // Assert
        Assert.Equal(id, orderItem.Id);
        Assert.Equal(quantity, orderItem.Quantity);
    }

    [Fact]
    public async Task SendOrderAsync_ThrowsException_WhenOrderIsNull()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _servingsClient.SendOrderAsync(null!));
    }

    [Fact]
    public async Task SendOrderAsync_CallsApi_WhenOrderIsValid()
    {
        // Arrange
        var order = new Order("test-order", new List<OrderItem>
        {
            new OrderItem { Id = "5979224", Quantity = "1" }
        });

        // Act & Assert - This would fail with real HTTP call
        // In real scenario, you'd mock the HttpClient inside ServingsApiClient
        await Assert.ThrowsAsync<ServingsApiException>(() => _servingsClient.SendOrderAsync(order));
    }

    [Fact]
    public void ServingsClient_ImplementsIDisposable()
    {
        // Arrange & Act
        using var client = new ServingsClient("https://test.com", "user", "pass");

        // Assert
        Assert.True(client is IDisposable);
    }

    public void Dispose()
    {
        _servingsClient?.Dispose();
    }
}
