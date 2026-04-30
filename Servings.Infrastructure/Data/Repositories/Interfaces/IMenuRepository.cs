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
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<List<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить блюдо по артикулу.
    /// </summary>
    /// <param name="article">Артикул блюда</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<MenuItem?> GetByArticleAsync(string article, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранить список блюд в меню.
    /// </summary>
    /// <param name="menuItems">Список блюд для сохранения</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task SaveMenuItemsAsync(List<MenuItem> menuItems, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить, существует ли блюдо с указанным артикулом.
    /// </summary>
    /// <param name="article">Артикул блюда</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<bool> ExistsByArticleAsync(string article, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить все блюда из меню.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task ClearAllAsync(CancellationToken cancellationToken = default);
}
