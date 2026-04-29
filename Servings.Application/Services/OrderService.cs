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
    public async Task<CreateOrderRequestDto> CreateOrderAsync()
    {
        try
        {
            await _logger.LogInfoAsync("Создание нового заказа...");
            
            var order = new CreateOrderRequestDto
            {
                Items = new List<OrderItemDto>()
            };

            await _logger.LogInfoAsync($"Заказ создан");
            return order;
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync("Ошибка при создании заказа", ex);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> AddItemToOrderAsync(CreateOrderRequestDto order, string article, int quantity)
    {
        try
        {
            await _logger.LogInfoAsync($"Добавление элемента в заказ: {article} - {quantity}");

            var menuItem = await _menuService.FindByArticleAsync(article);
            if (menuItem == null)
            {
                await _logger.LogWarningAsync($"Блюдо с артикулом {article} не найдено в меню");
                return false;
            }

            var orderItem = new OrderItemDto
            {
                Id = menuItem.ExternalId,
                Quantity = quantity
            };

            order.Items.Add(orderItem);

            await _logger.LogInfoAsync($"Элемент добавлен в заказ: {article} - {quantity}");
            return true;
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync($"Ошибка при добавлении элемента в заказ: {article}", ex);
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<OrderResponseDto> SendOrderAsync(CreateOrderRequestDto order)
    {
        try
        {
            await _logger.LogInfoAsync("Отправка заказа на сервер...");

            var response = await _apiClient.SendOrderAsync(order);

            if (response.Success)
            {
                await _logger.LogInfoAsync($"Заказ успешно отправлен. Server OrderId: {response.OrderId}");
            }
            else
            {
                await _logger.LogWarningAsync($"Ошибка при отправке заказа: {response.Message}");
            }

            return response;
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync("Ошибка при отправке заказа", ex);
            return new OrderResponseDto
            {
                Success = false,
                Message = $"Внутренняя ошибка: {ex.Message}"
            };
        }
    }
}
