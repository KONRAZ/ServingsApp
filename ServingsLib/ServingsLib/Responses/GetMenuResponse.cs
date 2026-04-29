using Newtonsoft.Json;
using ServingsLib.Models;

namespace ServingsLib.Responses;

public class GetMenuResponse : BaseResponse
{
    [JsonProperty("Data")]
    public MenuData? Data { get; set; }
}

public class MenuData
{
    [JsonProperty("MenuItems")]
    public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
