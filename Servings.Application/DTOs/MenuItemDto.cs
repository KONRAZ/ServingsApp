namespace Servings.Application.DTOs;

/// <summary>
/// DTO для элемента меню.
/// </summary>
public class MenuItemDto
{
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
