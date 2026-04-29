using Newtonsoft.Json;

namespace ServingsLib.Models;

public class MenuItem
{
    [JsonProperty("Id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("Article")]
    public string Article { get; set; } = string.Empty;

    [JsonProperty("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("Price")]
    public decimal Price { get; set; }

    [JsonProperty("IsWeighted")]
    public bool IsWeighted { get; set; }

    [JsonProperty("FullPath")]
    public string FullPath { get; set; } = string.Empty;

    [JsonProperty("Barcodes")]
    public List<string> Barcodes { get; set; } = new List<string>();
}
