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
    Task<CreateOrderRequestDto> CreateOrderAsync();

    /// <summary>
    /// Добавить элемент в заказ.
    /// </summary>
    Task<bool> AddItemToOrderAsync(CreateOrderRequestDto order, string article, int quantity);

    /// <summary>
    /// Отправить заказ на сервер.
    /// </summary>
    Task<OrderResponseDto> SendOrderAsync(CreateOrderRequestDto order);
}
