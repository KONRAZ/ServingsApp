using ServingsLib.Models;
using ServingsLib.Services;

namespace ServingsLib;

/// <summary>
/// Публичный клиент для работы с сервером заказов
/// </summary>
public class ServingsClient : IDisposable
{
    private readonly ServingsApiClient _apiClient;
    private bool _disposed = false;

    /// <summary>
    /// Инициализирует новый экземпляр клиента для работы с сервером заказов
    /// </summary>
    /// <param name="baseUrl">Базовый URL сервера</param>
    /// <param name="username">Имя пользователя для Basic аутентификации</param>
    /// <param name="password">Пароль для Basic аутентификации</param>
    /// <exception cref="ArgumentException">Выбрасывается при невалидных параметрах</exception>
    public ServingsClient(string baseUrl, string username, string password)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("Base URL cannot be null or empty", nameof(baseUrl));
        
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or empty", nameof(username));
        
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        _apiClient = new ServingsApiClient(baseUrl, username, password);
    }

    /// <summary>
    /// Получает меню с сервера
    /// </summary>
    /// <param name="withPrice">Включать ли цены в ответ</param>
    /// <returns>Список блюд</returns>
    /// <exception cref="Exceptions.ServingsApiException">Возникает при ошибке сервера или сетевой проблеме</exception>
    public async Task<List<MenuItem>> GetMenuAsync(bool withPrice = true)
    {
        return await _apiClient.GetMenuAsync(withPrice);
    }

    /// <summary>
    /// Отправляет заказ на сервер
    /// </summary>
    /// <param name="order">Заказ для отправки</param>
    /// <exception cref="ArgumentException">Возникает при невалидных параметрах заказа</exception>
    /// <exception cref="Exceptions.ServingsApiException">Возникает при ошибке сервера или сетевой проблеме</exception>
    public async Task SendOrderAsync(Order order)
    {
        await _apiClient.SendOrderAsync(order);
    }

    /// <summary>
    /// Создает новый заказ с указанными элементами
    /// </summary>
    /// <param name="orderId">Уникальный идентификатор заказа</param>
    /// <param name="menuItems">Список элементов заказа</param>
    /// <returns>Новый объект заказа</returns>
    public static Order CreateOrder(string orderId, List<OrderItem> menuItems)
    {
        return new Order(orderId, menuItems);
    }

    /// <summary>
    /// Создает элемент заказа
    /// </summary>
    /// <param name="id">ID блюда</param>
    /// <param name="quantity">Количество</param>
    /// <returns>Новый элемент заказа</returns>
    public static OrderItem CreateOrderItem(string id, string quantity)
    {
        return new OrderItem { Id = id, Quantity = quantity };
    }
    
    public void Dispose()
    {
        if (_disposed) return;
        _apiClient?.Dispose();
        _disposed = true;
    }
}
