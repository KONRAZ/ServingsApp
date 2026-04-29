using ServingsLib.Exceptions;
using ServingsLib.Services;
using Sms.Test;

namespace Tests.ServingsLibgRPC.Services;

public class SmsTestGrpcClientTests
{
    private readonly SmsTestGrpcClient _grpcClient;

    public SmsTestGrpcClientTests()
    {
        _grpcClient = new SmsTestGrpcClient("https://localhost:5001");
    }

    [Fact]
    public void Constructor_WithValidAddress_ShouldCreateClient()
    {
        // Act
        var client = new SmsTestGrpcClient("https://localhost:5001");

        // Assert
        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_WithNullAddress_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new SmsTestGrpcClient(null!));
    }

    [Fact]
    public void Constructor_WithEmptyAddress_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new SmsTestGrpcClient(""));
    }

    [Fact]
    public void Constructor_WithWhitespaceAddress_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new SmsTestGrpcClient("   "));
    }

    [Fact]
    public async Task GetMenuAsync_ShouldThrowServingsGrpcException_WhenServerNotAvailable()
    {
        // Arrange - используем несуществующий адрес
        
        // Act & Assert
        await Assert.ThrowsAsync<ServingsGrpcException>(() => _grpcClient.GetMenuAsync());
    }

    [Fact]
    public async Task GetMenuAsync_WithParameter_ShouldNotThrowArgumentNullException()
    {
        // Act & Assert - проверяем что метод принимает параметр
        await Assert.ThrowsAsync<ServingsGrpcException>(() => _grpcClient.GetMenuAsync(true));
    }

    [Fact]
    public async Task SendOrderAsync_WithValidOrder_ShouldThrowServingsGrpcException_WhenServerNotAvailable()
    {
        // Arrange
        var order = new Order
        {
            Id = "order-123"
        };
        order.OrderItems.Add(new OrderItem { Id = "5979224", Quantity = 1.0 });

        // Act & Assert
        await Assert.ThrowsAsync<ServingsGrpcException>(() => _grpcClient.SendOrderAsync(order));
    }

    [Fact]
    public async Task SendOrderAsync_WithNullOrder_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _grpcClient.SendOrderAsync(null!));
    }

    [Fact]
    public async Task SendOrderAsync_WithEmptyOrderId_ShouldThrowArgumentException()
    {
        // Arrange
        var order = new Order { Id = "" };
        order.OrderItems.Add(new OrderItem { Id = "5979224", Quantity = 1.0 });

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _grpcClient.SendOrderAsync(order));
    }

    [Fact]
    public async Task SendOrderAsync_WithNoOrderItems_ShouldThrowArgumentException()
    {
        // Arrange
        var order = new Order { Id = "order-123" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _grpcClient.SendOrderAsync(order));
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange
        var client = new SmsTestGrpcClient("https://localhost:5001");

        // Act & Assert
        Assert.Null(Record.Exception(() => client.Dispose()));
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes_ShouldNotThrow()
    {
        // Arrange
        var client = new SmsTestGrpcClient("https://localhost:5001");

        // Act & Assert
        Assert.Null(Record.Exception(() => client.Dispose()));
        Assert.Null(Record.Exception(() => client.Dispose()));
    }
}
