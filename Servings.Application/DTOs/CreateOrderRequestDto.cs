namespace Servings.Application.DTOs;

/// <summary>
/// DTO для создания заказа.
/// </summary>
public class CreateOrderRequestDto
{
    /// <summary>
    /// Элементы заказа.
    /// </summary>
    public List<OrderItemDto> Items { get; set; } = new();
}

/// <summary>
/// DTO для элемента заказа.
/// </summary>
public class OrderItemDto
{
    /// <summary>
    /// Артикул блюда.
    /// </summary>
    public string Article { get; set; } = string.Empty;

    /// <summary>
    /// Количество блюда.
    /// </summary>
    public int Quantity { get; set; }
}
