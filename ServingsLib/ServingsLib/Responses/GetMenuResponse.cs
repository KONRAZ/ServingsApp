using Newtonsoft.Json;
using ServingsLib.Models;

namespace ServingsLib.Responses;

/// <summary>
/// Ответ на запрос получения меню
/// </summary>
public class GetMenuResponse : BaseResponse
{
    /// <summary>
    /// Данные меню с списком блюд
    /// </summary>
    [JsonProperty("Data")]
    public MenuData? Data { get; set; }
}

/// <summary>
/// Контейнер для данных меню
/// </summary>
public class MenuData
{
    /// <summary>
    /// Список блюд в меню
    /// </summary>
    [JsonProperty("MenuItems")]
    public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
