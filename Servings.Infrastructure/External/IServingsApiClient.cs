using Servings.Infrastructure.DTOs;

namespace Servings.Infrastructure.External;

/// <summary>
/// Интерфейс для работы с внешним API Servings.
/// </summary>
public interface IServingsApiClient
{
    /// <summary>
    /// Получить меню с сервера.
    /// </summary>
    Task<List<MenuItemDto>> GetMenuAsync();

    /// <summary>
    /// Отправить заказ на сервер.
    /// </summary>
    Task<OrderResponseDto> SendOrderAsync(CreateOrderRequestDto request);
}
