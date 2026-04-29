using Newtonsoft.Json;

namespace ServingsLib.Responses;

/// <summary>
/// Базовый класс для всех ответов от API сервера
/// </summary>
public class BaseResponse
{
    /// <summary>
    /// Команда, которая была выполнена на сервере
    /// </summary>
    [JsonProperty("Command")]
    public string Command { get; set; } = string.Empty;

    /// <summary>
    /// Успешность выполнения команды
    /// </summary>
    [JsonProperty("Success")]
    public bool Success { get; set; }

    /// <summary>
    /// Сообщение об ошибке (если Success = false)
    /// </summary>
    [JsonProperty("ErrorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;
}
