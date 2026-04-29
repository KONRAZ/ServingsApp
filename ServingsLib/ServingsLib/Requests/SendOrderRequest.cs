using Newtonsoft.Json;
using ServingsLib.Models;

namespace ServingsLib.Requests;

public class SendOrderRequest
{
    [JsonProperty("Command")]
    public string Command { get; set; } = "SendOrder";

    [JsonProperty("CommandParameters")]
    public SendOrderParameters CommandParameters { get; set; } = new SendOrderParameters();
}

public class SendOrderParameters
{
    [JsonProperty("OrderId")]
    public string OrderId { get; set; } = string.Empty;

    [JsonProperty("MenuItems")]
    public List<OrderItem> MenuItems { get; set; } = new List<OrderItem>();

    public SendOrderParameters()
    {
    }

    public SendOrderParameters(string orderId, List<OrderItem> menuItems)
    {
        OrderId = orderId;
        MenuItems = menuItems ?? new List<OrderItem>();
    }
}
