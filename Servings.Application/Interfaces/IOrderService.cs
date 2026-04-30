using Servings.Infrastructure.DTOs;

namespace Servings.Application.Interfaces;

/// <summary>
/// Сервис для работы с заказами.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Создать новый заказ.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<CreateOrderRequestDto> CreateOrderAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить элемент в заказ.
    /// </summary>
    /// <param name="order">Заказ для добавления элемента</param>
    /// <param name="article">Артикул блюда</param>
    /// <param name="quantity">Количество</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<bool> AddItemToOrderAsync(CreateOrderRequestDto order, string article, int quantity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Отправить заказ на сервер.
    /// </summary>
    /// <param name="order">Заказ для отправки</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task<OrderResponseDto> SendOrderAsync(CreateOrderRequestDto order, CancellationToken cancellationToken = default);
}
