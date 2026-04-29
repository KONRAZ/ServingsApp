using ServingsLib;
using ServingsLib.Exceptions;
using Sms.Test;

namespace Tests.ServingsLibgRPC;

public class ServingsGrpcClientTests
{
    [Fact]
    public void Constructor_WithValidAddress_ShouldCreateClient()
    {
        // Act
        var client = new ServingsGrpcClient("https://localhost:5001");

        // Assert
        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_WithNullAddress_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ServingsGrpcClient(null!));
    }

    [Fact]
    public void Constructor_WithEmptyAddress_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ServingsGrpcClient(""));
    }

    [Fact]
    public void Constructor_WithWhitespaceAddress_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ServingsGrpcClient("   "));
    }

    [Fact]
    public async Task GetMenuAsync_ShouldCallGrpcClient()
    {
        // Arrange
        using var client = new ServingsGrpcClient("https://localhost:5001");

        // Act & Assert - пока просто проверяем что метод существует
        await Assert.ThrowsAsync<ServingsGrpcException>(() => client.GetMenuAsync());
    }

    [Fact]
    public async Task SendOrderAsync_ShouldCallGrpcClient()
    {
        // Arrange
        using var client = new ServingsGrpcClient("https://localhost:5001");
        var order = new Order { Id = "order-123" };
        order.OrderItems.Add(new OrderItem { Id = "5979224", Quantity = 1.0 });

        // Act & Assert - пока просто проверяем что метод существует
        await Assert.ThrowsAsync<ServingsGrpcException>(() => client.SendOrderAsync(order));
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange
        var client = new ServingsGrpcClient("https://localhost:5001");

        // Act & Assert
        Assert.Null(Record.Exception(() => client.Dispose()));
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes_ShouldNotThrow()
    {
        // Arrange
        var client = new ServingsGrpcClient("https://localhost:5001");

        // Act & Assert
        Assert.Null(Record.Exception(() => client.Dispose()));
        Assert.Null(Record.Exception(() => client.Dispose()));
    }
}
