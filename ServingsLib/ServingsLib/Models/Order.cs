using Newtonsoft.Json;

namespace ServingsLib.Models;

public class Order
{
    [JsonProperty("OrderId")]
    public string OrderId { get; set; } = string.Empty;

    [JsonProperty("MenuItems")]
    public List<OrderItem> MenuItems { get; set; } = new List<OrderItem>();

    public Order()
    {
    }

    public Order(string orderId, List<OrderItem> menuItems)
    {
        OrderId = orderId;
        MenuItems = menuItems ?? new List<OrderItem>();
    }
}
