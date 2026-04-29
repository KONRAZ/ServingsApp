namespace Servings.Infrastructure.DTOs;

/// <summary>
/// DTO для элемента меню.
/// </summary>
public class MenuItemDto
{
    /// <summary>
    /// Идентификатор блюда.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Название блюда.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Артикул блюда.
    /// </summary>
    public string Article { get; set; } = string.Empty;

    /// <summary>
    /// Цена блюда.
    /// </summary>
    public decimal Price { get; set; }
}
