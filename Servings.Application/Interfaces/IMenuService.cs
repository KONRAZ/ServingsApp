using Servings.Domain.Entities;

namespace Servings.Application.Interfaces;

/// <summary>
/// Сервис для работы с меню.
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// Получить меню с сервера и сохранить в базу данных.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<bool> LoadMenuFromServerAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все блюда из локальной базы данных.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<List<MenuItem>> GetLocalMenuAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Отобразить меню в консоль.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task DisplayMenuAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Найти блюдо по артикулу.
    /// </summary>
    /// <param name="article">Артикул блюда</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<MenuItem?> FindByArticleAsync(string article, CancellationToken cancellationToken = default);
}
