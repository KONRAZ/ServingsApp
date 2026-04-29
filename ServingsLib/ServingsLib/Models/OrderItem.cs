using Newtonsoft.Json;

namespace ServingsLib.Models;

/// <summary>
/// Представляет элемент заказа с указанием блюда и количества
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Идентификатор блюда из меню
    /// </summary>
    [JsonProperty("Id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Количество блюда (в виде строки для поддержки дробных значений)
    /// </summary>
    [JsonProperty("Quantity")]
    public string Quantity { get; set; } = string.Empty;
}
