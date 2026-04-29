using Newtonsoft.Json;

namespace ServingsLib.Models;

public class OrderItem
{
    [JsonProperty("Id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("Quantity")]
    public string Quantity { get; set; } = string.Empty;
}
