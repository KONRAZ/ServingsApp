using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Servings.Infrastructure.Data;

namespace Servings.ConsoleApp.Configuration;

/// <summary>
/// Реализация сервиса для инициализации базы данных.
/// </summary>
public class DbInitializer : IDbInitializer
{
    private readonly ApplicationContext _context;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(ApplicationContext context, ILogger<DbInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Инициализация базы данных...");

            // Проверяем, существует ли база данных
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
            
            if (!canConnect)
            {
                _logger.LogWarning("База данных недоступна. Создаем новую базу...");
                await _context.Database.EnsureCreatedAsync(cancellationToken);
                _logger.LogInformation("База данных создана");
            }
            else
            {
                // Применяем миграции, если база существует
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                
                if (pendingMigrations.Any())
                {
                    _logger.LogInformation("Применяем миграции...");
                    await _context.Database.MigrateAsync(cancellationToken);
                    _logger.LogInformation("Миграции успешно применены");
                }
                else
                {
                    _logger.LogInformation("База данных актуальна");
                }
            }

            _logger.LogInformation("Инициализация базы данных завершена");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при инициализации базы данных");
            throw;
        }
    }
}
