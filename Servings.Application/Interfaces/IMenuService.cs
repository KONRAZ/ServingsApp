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
    Task<bool> LoadMenuFromServerAsync();

    /// <summary>
    /// Получить все блюда из локальной базы данных.
    /// </summary>
    Task<List<MenuItem>> GetLocalMenuAsync();

    /// <summary>
    /// Отобразить меню в консоль.
    /// </summary>
    Task DisplayMenuAsync();

    /// <summary>
    /// Найти блюдо по артикулу.
    /// </summary>
    Task<MenuItem?> FindByArticleAsync(string article);
}
