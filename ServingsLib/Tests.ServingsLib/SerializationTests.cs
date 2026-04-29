using Newtonsoft.Json;
using ServingsLib.Requests;
using ServingsLib.Responses;

namespace Tests.ServingsLib;

public class SerializationTests
{
    [Fact]
    public void GetMenuRequest_SerializesCorrectly()
    {
        // Arrange
        var request = new GetMenuRequest
        {
            CommandParameters = new GetMenuParameters { WithPrice = true }
        };

        // Act
        var json = JsonConvert.SerializeObject(request, Formatting.Indented);

        // Assert
        Assert.Contains("\"Command\": \"GetMenu\"", json);
        Assert.Contains("\"WithPrice\": true", json);
    }

    [Fact]
    public void GetMenuResponse_DeserializesCorrectly()
    {
        // Arrange
        var json = @"{
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
                    },
                    {
                        ""Id"": ""9084246"",
                        ""Article"": ""A1004293"",
                        ""Name"": ""Конфеты Коровка"",
                        ""Price"": 300,
                        ""IsWeighted"": true,
                        ""FullPath"": ""ДЕСЕРТЫ\\Развес"",
                        ""Barcodes"": []
                    }
                ]
            }
        }";

        // Act
        var response = JsonConvert.DeserializeObject<GetMenuResponse>(json);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Equal("", response.ErrorMessage);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.MenuItems.Count);

        var firstItem = response.Data.MenuItems[0];
        Assert.Equal("5979224", firstItem.Id);
        Assert.Equal("A1004292", firstItem.Article);
        Assert.Equal("Каша гречневая", firstItem.Name);
        Assert.Equal(50m, firstItem.Price);
        Assert.False(firstItem.IsWeighted);
        Assert.Equal("ПРОИЗВОДСТВО\\Гарниры", firstItem.FullPath);
        Assert.Single(firstItem.Barcodes);
        Assert.Equal("57890975627974236429", firstItem.Barcodes[0]);

        var secondItem = response.Data.MenuItems[1];
        Assert.Equal("9084246", secondItem.Id);
        Assert.Equal("A1004293", secondItem.Article);
        Assert.Equal("Конфеты Коровка", secondItem.Name);
        Assert.Equal(300m, secondItem.Price);
        Assert.True(secondItem.IsWeighted);
        Assert.Equal("ДЕСЕРТЫ\\Развес", secondItem.FullPath);
        Assert.Empty(secondItem.Barcodes);
    }

    [Fact]
    public void SendOrderRequest_SerializesCorrectly()
    {
        // Arrange
        var request = new SendOrderRequest
        {
            CommandParameters = new SendOrderParameters(
                "62137983-1117-4D10-87C1-EF40A4348250",
                new List<OrderItem>
                {
                    new OrderItem { Id = "5979224", Quantity = "1" },
                    new OrderItem { Id = "9084246", Quantity = "0.408" }
                })
        };

        // Act
        var json = JsonConvert.SerializeObject(request, Formatting.Indented);

        // Assert
        Assert.Contains("\"Command\": \"SendOrder\"", json);
        Assert.Contains("\"OrderId\": \"62137983-1117-4D10-87C1-EF40A4348250\"", json);
        Assert.Contains("\"Id\": \"5979224\"", json);
        Assert.Contains("\"Quantity\": \"1\"", json);
        Assert.Contains("\"Id\": \"9084246\"", json);
        Assert.Contains("\"Quantity\": \"0.408\"", json);
    }

    [Fact]
    public void SendOrderResponse_DeserializesCorrectly()
    {
        // Arrange
        var json = @"{
            ""Command"": ""SendOrder"",
            ""Success"": true,
            ""ErrorMessage"": """"
        }";

        // Act
        var response = JsonConvert.DeserializeObject<SendOrderResponse>(json);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Equal("", response.ErrorMessage);
        Assert.Equal("SendOrder", response.Command);
    }

    [Fact]
    public void ErrorResponse_DeserializesCorrectly()
    {
        // Arrange
        var json = @"{
            ""Command"": ""GetMenu"",
            ""Success"": false,
            ""ErrorMessage"": ""Database connection failed""
        }";

        // Act
        var response = JsonConvert.DeserializeObject<GetMenuResponse>(json);

        // Assert
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Equal("Database connection failed", response.ErrorMessage);
        Assert.Equal("GetMenu", response.Command);
        Assert.Null(response.Data);
    }

    [Fact]
    public void MenuItem_HandlesEmptyBarcodesArray()
    {
        // Arrange
        var json = @"{
            ""Id"": ""9084246"",
            ""Article"": ""A1004293"",
            ""Name"": ""Конфеты Коровка"",
            ""Price"": 300,
            ""IsWeighted"": true,
            ""FullPath"": ""ДЕСЕРТЫ\\Развес"",
            ""Barcodes"": []
        }";

        // Act
        var menuItem = JsonConvert.DeserializeObject<MenuItem>(json);

        // Assert
        Assert.NotNull(menuItem);
        Assert.NotNull(menuItem.Barcodes);
        Assert.Empty(menuItem.Barcodes);
    }

    [Fact]
    public void MenuItem_HandlesNullBarcodesArray()
    {
        // Arrange
        var json = @"{
            ""Id"": ""9084246"",
            ""Article"": ""A1004293"",
            ""Name"": ""Конфеты Коровка"",
            ""Price"": 300,
            ""IsWeighted"": true,
            ""FullPath"": ""ДЕСЕРТЫ\\Развес"",
            ""Barcodes"": null
        }";

        // Act
        var menuItem = JsonConvert.DeserializeObject<MenuItem>(json);

        // Assert
        Assert.NotNull(menuItem);
        // Newtonsoft.Json оставляет null для null значений в JSON
        Assert.Null(menuItem.Barcodes);
    }
}
