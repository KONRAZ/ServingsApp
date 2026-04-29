using Servings.Domain.Entities;
using Servings.Infrastructure.DTOs;

namespace Servings.Application.Mappings;

/// <summary>
/// Маппинг для MenuItem.
/// </summary>
public static class MenuItemMapping
{
    /// <summary>
    /// Преобразовать список MenuItemDto в список MenuItem.
    /// </summary>
    public static List<MenuItem> ToDomainList(this List<MenuItemDto> dtos)
    {
        return dtos.Select(ToDomain).ToList();
    }

    /// <summary>
    /// Преобразовать MenuItemDto в MenuItem.
    /// </summary>
    private static MenuItem ToDomain(this MenuItemDto dto)
    {
        return new MenuItem
        {
            ExternalId = dto.Id, // Используем Id из ServingsLib как ExternalId
            Name = dto.Name,
            Article = dto.Article,
            Price = dto.Price
        };
    }
}
