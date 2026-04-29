namespace Servings.Domain.Entities;

/// <summary>
/// Элемент заказа, связанный с блюдом из меню.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Уникальный идентификатор элемента заказа.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Код (артикул) блюда из меню (внешний ключ).
    /// </summary>
    public string Article { get; set; } = string.Empty;

    /// <summary>
    /// Количество блюда в заказе.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Связанное блюдо из меню (для навигации).
    /// </summary>
    public MenuItem? MenuItem { get; set; }
}
