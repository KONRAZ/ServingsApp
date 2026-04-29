using System.ComponentModel.DataAnnotations.Schema;

namespace Servings.Domain.Entities;

/// <summary>
/// Блюдо в меню.
/// </summary>
[Table("menu-items")]
public class MenuItem
{
    /// <summary>
    /// Уникальный идентификатор блюда.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Внешний идентификатор блюда из ServingsLib.
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Название блюда.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Код (артикул) блюда.
    /// </summary>
    public string Article { get; set; } = string.Empty;

    /// <summary>
    /// Цена за единицу блюда.
    /// </summary>
    public decimal Price { get; set; }
}
