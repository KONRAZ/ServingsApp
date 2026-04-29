using Servings.Domain.Entities;

namespace Servings.Infrastructure.Data.Repositories.Interfaces;

/// <summary>
/// Репозиторий для работы с блюдами меню.
/// </summary>
public interface IMenuRepository
{
    /// <summary>
    /// Получить все блюда из меню.
    /// </summary>
    Task<List<MenuItem>> GetAllAsync();

    /// <summary>
    /// Получить блюдо по артикулу.
    /// </summary>
    Task<MenuItem?> GetByArticleAsync(string article);

    /// <summary>
    /// Сохранить список блюд в меню.
    /// </summary>
    Task SaveMenuItemsAsync(List<MenuItem> menuItems);

    /// <summary>
    /// Проверить, существует ли блюдо с указанным артикулом.
    /// </summary>
    Task<bool> ExistsByArticleAsync(string article);

    /// <summary>
    /// Удалить все блюда из меню.
    /// </summary>
    Task ClearAllAsync();
}
