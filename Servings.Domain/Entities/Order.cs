namespace Servings.Domain.Entities;

/// <summary>
/// Представляет заказ.
/// </summary>
public class Order
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Список элементов заказа.
    /// </summary>
    public List<OrderItem> OrderItems { get; set; } = new();
}
