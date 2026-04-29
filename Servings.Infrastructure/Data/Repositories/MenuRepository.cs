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
    public async Task<List<MenuItem>> GetAllAsync()
    {
        return await _context.MenuItems
            .OrderBy(m => m.Article)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<MenuItem?> GetByArticleAsync(string article)
    {
        return await _context.MenuItems
            .FirstOrDefaultAsync(m => m.Article == article);
    }

    /// <inheritdoc/>
    public async Task SaveMenuItemsAsync(List<MenuItem> menuItems)
    {
        if (menuItems == null || !menuItems.Any())
            return;

        // Очищаем существующие блюда
        await ClearAllAsync();

        // Добавляем новые блюда
        await _context.MenuItems.AddRangeAsync(menuItems);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByArticleAsync(string article)
    {
        return await _context.MenuItems
            .AnyAsync(m => m.Article == article);
    }

    /// <inheritdoc/>
    public async Task ClearAllAsync()
    {
        var existingItems = await _context.MenuItems.ToListAsync();
        if (existingItems.Any())
        {
            _context.MenuItems.RemoveRange(existingItems);
            await _context.SaveChangesAsync();
        }
    }
}
