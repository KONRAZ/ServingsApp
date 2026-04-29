using Servings.Domain.Entities;

namespace Servings.Infrastructure.Data.Repositories.Interfaces;

/// <summary>
/// Репозиторий для работы с заказами.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Получить все заказы.
    /// </summary>
    Task<List<Order>> GetAllAsync();

    /// <summary>
    /// Получить заказ по идентификатору.
    /// </summary>
    Task<Order?> GetByIdAsync(int id);

    /// <summary>
    /// Сохранить заказ.
    /// </summary>
    Task<int> SaveAsync(Order order);

    /// <summary>
    /// Удалить заказ по идентификатору.
    /// </summary>
    Task<bool> DeleteAsync(int id);
}
