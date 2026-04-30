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
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<List<MenuItemDto>> GetMenuAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Отправить заказ на сервер.
    /// </summary>
    /// <param name="request">Запрос на создание заказа</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<OrderResponseDto> SendOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default);
}
