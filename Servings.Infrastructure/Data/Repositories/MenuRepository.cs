using Microsoft.EntityFrameworkCore;
using Servings.Domain.Entities;
using Servings.Infrastructure.Data.Repositories.Interfaces;

namespace Servings.Infrastructure.Data.Repositories;

/// <summary>
/// Реализация репозитория для работы с блюдами меню.
/// </summary>
public class MenuRepository : IMenuRepository
{
    private readonly ApplicationContext _context;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="context"></param>
    public MenuRepository(ApplicationContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<List<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MenuItems
            .OrderBy(m => m.Article)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<MenuItem?> GetByArticleAsync(string article, CancellationToken cancellationToken = default)
    {
        return await _context.MenuItems
            .FirstOrDefaultAsync(m => m.Article == article, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SaveMenuItemsAsync(List<MenuItem> menuItems, CancellationToken cancellationToken = default)
    {
        if (menuItems == null || !menuItems.Any())
            return;

        // Очищаем существующие блюда
        await ClearAllAsync(cancellationToken);

        // Добавляем новые блюда
        await _context.MenuItems.AddRangeAsync(menuItems, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByArticleAsync(string article, CancellationToken cancellationToken = default)
    {
        return await _context.MenuItems
            .AnyAsync(m => m.Article == article, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task ClearAllAsync(CancellationToken cancellationToken = default)
    {
        var existingItems = await _context.MenuItems.ToListAsync(cancellationToken);
        if (existingItems.Any())
        {
            _context.MenuItems.RemoveRange(existingItems);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
