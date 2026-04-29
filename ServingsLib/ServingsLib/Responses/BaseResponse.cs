using Newtonsoft.Json;

namespace ServingsLib.Responses;

public class BaseResponse
{
    [JsonProperty("Command")]
    public string Command { get; set; } = string.Empty;

    [JsonProperty("Success")]
    public bool Success { get; set; }

    [JsonProperty("ErrorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;
}
