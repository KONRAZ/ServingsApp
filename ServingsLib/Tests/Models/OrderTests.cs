using ServingsLib.Models;

namespace Tests.Models;

public class OrderTests
{
    [Fact]
    public void Order_CanBeCreated_WithDefaultConstructor()
    {
        // Arrange & Act
        var order = new Order();

        // Assert
        Assert.Equal(string.Empty, order.OrderId);
        Assert.Empty(order.MenuItems);
    }

    [Fact]
    public void Order_CanBeCreated_WithParameters()
    {
        // Arrange
        var orderId = "62137983-1117-4D10-87C1-EF40A4348250";
        var menuItems = new List<OrderItem>
        {
            new OrderItem { Id = "5979224", Quantity = "1" },
            new OrderItem { Id = "9084246", Quantity = "0.408" }
        };

        // Act
        var order = new Order(orderId, menuItems);

        // Assert
        Assert.Equal(orderId, order.OrderId);
        Assert.Equal(menuItems, order.MenuItems);
    }

    [Fact]
    public void Order_WithNullMenuItems_ShouldCreateEmptyList()
    {
        // Arrange
        var orderId = "test-order";

        // Act
        var order = new Order(orderId, null);

        // Assert
        Assert.Equal(orderId, order.OrderId);
        Assert.Empty(order.MenuItems);
    }

    [Fact]
    public void OrderItem_CanBeCreated_WithDefaultValues()
    {
        // Arrange & Act
        var orderItem = new OrderItem();

        // Assert
        Assert.Equal(string.Empty, orderItem.Id);
        Assert.Equal(string.Empty, orderItem.Quantity);
    }

    [Fact]
    public void OrderItem_CanSetProperties()
    {
        // Arrange
        var orderItem = new OrderItem();

        // Act
        orderItem.Id = "5979224";
        orderItem.Quantity = "1";

        // Assert
        Assert.Equal("5979224", orderItem.Id);
        Assert.Equal("1", orderItem.Quantity);
    }
}
