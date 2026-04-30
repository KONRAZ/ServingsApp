using Servings.Infrastructure.DTOs;
using Servings.Application.Interfaces;
using Servings.Infrastructure.External;
using Servings.Infrastructure.Logging;

namespace Servings.Application.Services;

/// <summary>
/// Реализация сервиса для работы с заказами.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IServingsApiClient _apiClient;
    private readonly IMenuService _menuService;
    private readonly IFileLogger _logger;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="apiClient"></param>
    /// <param name="menuService"></param>
    /// <param name="logger"></param>
    public OrderService(
        IServingsApiClient apiClient,
        IMenuService menuService,
        IFileLogger logger)
    {
        _apiClient = apiClient;
        _menuService = menuService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CreateOrderRequestDto> CreateOrderAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _logger.LogInfoAsync("Создание нового заказа...", cancellationToken);
            
            var order = new CreateOrderRequestDto
            {
                Items = new List<OrderItemDto>()
            };

            await _logger.LogInfoAsync("Заказ создан", cancellationToken);
            return order;
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync("Ошибка при создании заказа", ex, cancellationToken);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> AddItemToOrderAsync(CreateOrderRequestDto order, string article, int quantity, CancellationToken cancellationToken = default)
    {
        try
        {
            await _logger.LogInfoAsync($"Добавление элемента в заказ: {article} - {quantity}", cancellationToken);

            var menuItem = await _menuService.FindByArticleAsync(article, cancellationToken);
            if (menuItem == null)
            {
                await _logger.LogWarningAsync($"Блюдо с артикулом {article} не найдено в меню", cancellationToken);
                return false;
            }

            var orderItem = new OrderItemDto
            {
                Id = menuItem.ExternalId,
                Quantity = quantity
            };

            order.Items.Add(orderItem);

            await _logger.LogInfoAsync($"Элемент добавлен в заказ: {article} - {quantity}", cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync($"Ошибка при добавлении элемента в заказ: {article}", ex, cancellationToken);
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<OrderResponseDto> SendOrderAsync(CreateOrderRequestDto order, CancellationToken cancellationToken = default)
    {
        try
        {
            await _logger.LogInfoAsync("Отправка заказа на сервер...", cancellationToken);

            var response = await _apiClient.SendOrderAsync(order, cancellationToken);

            if (response.Success)
            {
                await _logger.LogInfoAsync($"Заказ успешно отправлен. Server OrderId: {response.OrderId}", cancellationToken);
            }
            else
            {
                await _logger.LogWarningAsync($"Ошибка при отправке заказа: {response.Message}", cancellationToken);
            }

            return response;
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync("Ошибка при отправке заказа", ex, cancellationToken);
            return new OrderResponseDto
            {
                Success = false,
                Message = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }
}
