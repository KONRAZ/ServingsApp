using Newtonsoft.Json;
using ServingsLib.Models;

namespace ServingsLib.Requests;

/// <summary>
/// Запрос для отправки заказа на сервер
/// </summary>
public class SendOrderRequest
{
    /// <summary>
    /// Команда отправки заказа
    /// </summary>
    [JsonProperty("Command")]
    public string Command { get; set; } = "SendOrder";

    /// <summary>
    /// Параметры запроса отправки заказа
    /// </summary>
    [JsonProperty("CommandParameters")]
    public SendOrderParameters CommandParameters { get; set; } = new SendOrderParameters();
}

/// <summary>
/// Параметры для запроса отправки заказа
/// </summary>
public class SendOrderParameters
{
    /// <summary>
    /// Уникальный идентификатор заказа
    /// </summary>
    [JsonProperty("OrderId")]
    public string OrderId { get; set; } = string.Empty;

    /// <summary>
    /// Список блюд в заказе
    /// </summary>
    [JsonProperty("MenuItems")]
    public List<OrderItem> MenuItems { get; set; } = new List<OrderItem>();

    /// <summary>
    /// Инициализирует пустые параметры заказа
    /// </summary>
    public SendOrderParameters()
    {
    }

    /// <summary>
    /// Инициализирует параметры заказа с указанными значениями
    /// </summary>
    /// <param name="orderId">Уникальный идентификатор заказа</param>
    /// <param name="menuItems">Список блюд в заказе</param>
    public SendOrderParameters(string orderId, List<OrderItem> menuItems)
    {
        OrderId = orderId;
        MenuItems = menuItems ?? new List<OrderItem>();
    }
}
