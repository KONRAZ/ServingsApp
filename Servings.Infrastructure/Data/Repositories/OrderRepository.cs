using Microsoft.EntityFrameworkCore;
using Servings.Domain.Entities;
using Servings.Infrastructure.Data.Repositories.Interfaces;

namespace Servings.Infrastructure.Data.Repositories;

/// <summary>
/// Реализация репозитория для работы с заказами.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly ApplicationContext _context;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="context"></param>
    public OrderRepository(ApplicationContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .OrderByDescending(o => o.Id)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    /// <inheritdoc/>
    public async Task<int> SaveAsync(Order order)
    {
        if (order.Id == 0)
        {
            // Новый заказ
            await _context.Orders.AddAsync(order);
        }
        else
        {
            // Существующий заказ
            _context.Orders.Update(order);
        }

        await _context.SaveChangesAsync();
        return order.Id;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
}
