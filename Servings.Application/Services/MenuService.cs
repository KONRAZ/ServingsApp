using Servings.Domain.Entities;
using Servings.Application.Interfaces;
using Servings.Application.Mappings;
using Servings.Infrastructure.Data.Repositories.Interfaces;
using Servings.Infrastructure.External;
using Servings.Infrastructure.Logging;

namespace Servings.Application.Services;

/// <summary>
/// Реализация сервиса для работы с меню.
/// </summary>
public class MenuService : IMenuService
{
    private readonly IServingsApiClient _apiClient;
    private readonly IMenuRepository _menuRepository;
    private readonly IFileLogger _logger;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="apiClient"></param>
    /// <param name="menuRepository"></param>
    /// <param name="logger"></param>
    public MenuService(
        IServingsApiClient apiClient,
        IMenuRepository menuRepository,
        IFileLogger logger)
    {
        _apiClient = apiClient;
        _menuRepository = menuRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<bool> LoadMenuFromServerAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _logger.LogInfoAsync("Загрузка меню с сервера...", cancellationToken);

            var menuDtos = await _apiClient.GetMenuAsync(cancellationToken);
            var menuItems = menuDtos.ToDomainList();

            await _menuRepository.SaveMenuItemsAsync(menuItems, cancellationToken);

            await _logger.LogInfoAsync($"Меню успешно загружено. Количество блюд: {menuItems.Count}", cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync("Ошибка при загрузке меню с сервера", ex, cancellationToken);
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<List<MenuItem>> GetLocalMenuAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var menuItems = await _menuRepository.GetAllAsync(cancellationToken);
            return menuItems;
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync("Ошибка при получении локального меню", ex, cancellationToken);
            return new List<MenuItem>();
        }
    }

    /// <inheritdoc/>
    public async Task DisplayMenuAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var menuItems = await GetLocalMenuAsync(cancellationToken);
            
            if (!menuItems.Any())
            {
                Console.WriteLine("Меню пусто. Загрузите меню с сервера.");
                return;
            }

            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("Артикул | Название | Цена");
            Console.WriteLine(new string('-', 50));

            foreach (var item in menuItems.OrderBy(m => m.Article))
            {
                Console.WriteLine($"{item.Article,-8} | {item.Name,-20} | {item.Price:F2}");
            }
            
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"Всего блюд: {menuItems.Count}");
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync("Ошибка при отображении меню", ex, cancellationToken);
            Console.WriteLine("Ошибка при отображении меню");
        }
    }

    /// <inheritdoc/>
    public async Task<MenuItem?> FindByArticleAsync(string article, CancellationToken cancellationToken = default)
    {
        try
        {
            var menuItems = await GetLocalMenuAsync(cancellationToken);
            return menuItems.FirstOrDefault(m => 
                m.Article.Equals(article, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync($"Ошибка при поиске блюда по артикулу: {article}", ex, cancellationToken);
            return null;
        }
    }
}
