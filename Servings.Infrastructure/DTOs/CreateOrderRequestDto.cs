namespace Servings.Infrastructure.DTOs;

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
    /// Id блюда из меню.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Количество блюда.
    /// </summary>
    public int Quantity { get; set; }
}
