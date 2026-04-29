using Newtonsoft.Json;

namespace ServingsLib.Requests;

public class GetMenuRequest
{
    [JsonProperty("Command")]
    public string Command { get; set; } = "GetMenu";

    [JsonProperty("CommandParameters")]
    public GetMenuParameters CommandParameters { get; set; } = new GetMenuParameters();
}

public class GetMenuParameters
{
    [JsonProperty("WithPrice")]
    public bool WithPrice { get; set; } = true;
}
