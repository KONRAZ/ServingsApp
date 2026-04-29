using Newtonsoft.Json;

namespace ServingsLib.Requests;

/// <summary>
/// Базовый класс для всех запросов к API сервера
/// </summary>
public class BaseRequest
{
    /// <summary>
    /// Команда для выполнения на сервере
    /// </summary>
    [JsonProperty("Command")]
    public string Command { get; set; } = string.Empty;

    /// <summary>
    /// Параметры команды
    /// </summary>
    [JsonProperty("CommandParameters")]
    public object CommandParameters { get; set; } = new object();
}
