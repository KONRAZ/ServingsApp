using Newtonsoft.Json;

namespace ServingsLib.Requests;

public class BaseRequest
{
    [JsonProperty("Command")]
    public string Command { get; set; } = string.Empty;

    [JsonProperty("CommandParameters")]
    public object CommandParameters { get; set; } = new object();
}
