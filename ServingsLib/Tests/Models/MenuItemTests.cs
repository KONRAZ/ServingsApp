using ServingsLib.Models;

namespace Tests.Models;

public class MenuItemTests
{
    [Fact]
    public void MenuItem_CanBeCreated_WithDefaultValues()
    {
        // Arrange & Act
        var menuItem = new MenuItem();

        // Assert
        Assert.Equal(string.Empty, menuItem.Id);
        Assert.Equal(string.Empty, menuItem.Article);
        Assert.Equal(string.Empty, menuItem.Name);
        Assert.Equal(0m, menuItem.Price);
        Assert.False(menuItem.IsWeighted);
        Assert.Equal(string.Empty, menuItem.FullPath);
        Assert.Empty(menuItem.Barcodes);
    }

    [Fact]
    public void MenuItem_CanSetProperties()
    {
        // Arrange
        var menuItem = new MenuItem();
        var barcodes = new List<string> { "123456789", "987654321" };

        // Act
        menuItem.Id = "5979224";
        menuItem.Article = "A1004292";
        menuItem.Name = "Каша гречневая";
        menuItem.Price = 50;
        menuItem.IsWeighted = false;
        menuItem.FullPath = "ПРОИЗВОДСТВО\\Гарниры";
        menuItem.Barcodes = barcodes;

        // Assert
        Assert.Equal("5979224", menuItem.Id);
        Assert.Equal("A1004292", menuItem.Article);
        Assert.Equal("Каша гречневая", menuItem.Name);
        Assert.Equal(50m, menuItem.Price);
        Assert.False(menuItem.IsWeighted);
        Assert.Equal("ПРОИЗВОДСТВО\\Гарниры", menuItem.FullPath);
        Assert.Equal(barcodes, menuItem.Barcodes);
    }
}
