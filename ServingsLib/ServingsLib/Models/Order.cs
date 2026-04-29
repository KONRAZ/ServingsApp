using Newtonsoft.Json;

namespace ServingsLib.Models;

/// <summary>
/// Представляет заказ с уникальным идентификатором и списком блюд
/// </summary>
public class Order
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
    /// Инициализирует пустой заказ
    /// </summary>
    public Order()
    {
    }

    /// <summary>
    /// Инициализирует заказ с указанными параметрами
    /// </summary>
    /// <param name="orderId">Уникальный идентификатор заказа</param>
    /// <param name="menuItems">Список блюд в заказе</param>
    public Order(string orderId, List<OrderItem> menuItems)
    {
        OrderId = orderId;
        MenuItems = menuItems ?? new List<OrderItem>();
    }
}
