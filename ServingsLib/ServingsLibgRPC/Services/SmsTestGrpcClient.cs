using Grpc.Net.Client;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Sms.Test;
using ServingsLib.Exceptions;

namespace ServingsLib.Services;

/// <summary>
/// gRPC клиент для взаимодействия с API сервера заказов
/// </summary>
public class SmsTestGrpcClient : IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly SmsTestService.SmsTestServiceClient _client;
    private bool _disposed = false;

    /// <summary>
    /// Инициализирует новый экземпляр gRPC клиента
    /// </summary>
    /// <param name="serverAddress">Адрес gRPC сервера</param>
    /// <exception cref="ArgumentException">Выбрасывается при невалидных параметрах</exception>
    public SmsTestGrpcClient(string serverAddress)
    {
        if (string.IsNullOrWhiteSpace(serverAddress))
            throw new ArgumentException("Server address cannot be null or empty", nameof(serverAddress));

        _channel = GrpcChannel.ForAddress(serverAddress);
        _client = new SmsTestService.SmsTestServiceClient(_channel);
    }

    /// <summary>
    /// Получает меню с сервера
    /// </summary>
    /// <param name="withPrice">Включать ли цены в ответ</param>
    /// <returns>Список блюд</returns>
    /// <exception cref="ServingsGrpcException">Выбрасывается при ошибке gRPC сервера или сетевой проблеме</exception>
    public async Task<List<MenuItem>> GetMenuAsync(bool withPrice = true)
    {
        try
        {
            var request = new BoolValue { Value = withPrice };
            var response = await _client.GetMenuAsync(request);

            return !response.Success ? 
                throw new ServingsGrpcException($"Server returned error: {response.ErrorMessage}", response.ErrorMessage) 
                : response.MenuItems.ToList();
        }
        catch (RpcException ex)
        {
            throw new ServingsGrpcException("gRPC request failed", ex);
        }
        catch (Exception ex)
        {
            throw new ServingsGrpcException("Unexpected error occurred", ex);
        }
    }

    /// <summary>
    /// Отправляет заказ на сервер
    /// </summary>
    /// <param name="order">Заказ для отправки</param>
    /// <exception cref="ArgumentException">Выбрасывается при невалидных параметрах заказа</exception>
    /// <exception cref="ServingsGrpcException">Выбрасывается при ошибке gRPC сервера или сетевой проблеме</exception>
    public async Task SendOrderAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (string.IsNullOrWhiteSpace(order.Id))
        {
            throw new ArgumentException("Order Id is required", nameof(order));
        }

        if (order.OrderItems == null || !order.OrderItems.Any())
        {
            throw new ArgumentException("Order must contain at least one menu item", nameof(order));
        }

        try
        {
            var response = await _client.SendOrderAsync(order);

            if (!response.Success)
            {
                throw new ServingsGrpcException($"Server returned error: {response.ErrorMessage}", response.ErrorMessage);
            }
        }
        catch (RpcException ex)
        {
            throw new ServingsGrpcException("gRPC request failed", ex);
        }
        catch (Exception ex)
        {
            throw new ServingsGrpcException("Unexpected error occurred", ex);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _channel?.Dispose();
        _disposed = true;
    }
}
