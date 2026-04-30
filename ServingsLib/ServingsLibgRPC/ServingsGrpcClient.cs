using ServingsLib.Services;
using Sms.Test;

namespace ServingsLib;

/// <summary>
/// Публичный клиент для работы с сервером заказов через gRPC
/// </summary>
public class ServingsGrpcClient : IDisposable
{
    private readonly SmsTestGrpcClient _grpcClient;
    private bool _disposed = false;

    /// <summary>
    /// Инициализирует новый экземпляр клиента для работы с сервером заказов через gRPC
    /// </summary>
    /// <param name="serverAddress">Адрес gRPC сервера</param>
    /// <exception cref="ArgumentException">Выбрасывается при невалидных параметрах</exception>
    public ServingsGrpcClient(string serverAddress)
    {
        if (string.IsNullOrWhiteSpace(serverAddress))
            throw new ArgumentException("Server address cannot be null or empty", nameof(serverAddress));

        _grpcClient = new SmsTestGrpcClient(serverAddress);
    }

    /// <summary>
    /// Получает меню с сервера
    /// </summary>
    /// <param name="withPrice">Включать ли цены в ответ</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список блюд</returns>
    /// <exception cref="Exceptions.ServingsGrpcException">Возникает при ошибке сервера или сетевой проблеме</exception>
    public async Task<List<MenuItem>> GetMenuAsync(bool withPrice = true, CancellationToken cancellationToken = default)
    {
        return await _grpcClient.GetMenuAsync(withPrice, cancellationToken);
    }

    /// <summary>
    /// Отправляет заказ на сервер
    /// </summary>
    /// <param name="order">Заказ для отправки</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <exception cref="ArgumentException">Возникает при невалидных параметрах заказа</exception>
    /// <exception cref="Exceptions.ServingsGrpcException">Возникает при ошибке сервера или сетевой проблеме</exception>
    public async Task SendOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _grpcClient.SendOrderAsync(order, cancellationToken);
    }
    
    public void Dispose()
    {
        if (_disposed) return;
        _grpcClient?.Dispose();
        _disposed = true;
    }
}
