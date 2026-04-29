using Newtonsoft.Json;

namespace ServingsLib.Requests;

/// <summary>
/// Запрос для получения меню с сервера
/// </summary>
public class GetMenuRequest
{
    /// <summary>
    /// Команда получения меню
    /// </summary>
    [JsonProperty("Command")]
    public string Command { get; set; } = "GetMenu";

    /// <summary>
    /// Параметры запроса меню
    /// </summary>
    [JsonProperty("CommandParameters")]
    public GetMenuParameters CommandParameters { get; set; } = new GetMenuParameters();
}

/// <summary>
/// Параметры для запроса получения меню
/// </summary>
public class GetMenuParameters
{
    /// <summary>
    /// Включать ли цены в ответ
    /// </summary>
    [JsonProperty("WithPrice")]
    public bool WithPrice { get; set; } = true;
}
