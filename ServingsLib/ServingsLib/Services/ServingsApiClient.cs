using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ServingsLib.Exceptions;
using ServingsLib.Models;
using ServingsLib.Requests;
using ServingsLib.Responses;

namespace ServingsLib.Services;

/// <summary>
/// HTTP клиент для взаимодействия с API сервера заказов
/// </summary>
public class ServingsApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private bool _disposed = false;

    /// <summary>
    /// Инициализирует новый экземпляр HTTP клиента
    /// </summary>
    /// <param name="baseUrl">Базовый URL сервера</param>
    /// <param name="username">Имя пользователя для Basic аутентификации</param>
    /// <param name="password">Пароль для Basic аутентификации</param>
    /// <exception cref="ArgumentException">Выбрасывается при невалидных параметрах</exception>
    public ServingsApiClient(string baseUrl, string username, string password)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("Base URL cannot be null or empty", nameof(baseUrl));
        
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or empty", nameof(username));
        
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        _baseUrl = baseUrl.TrimEnd('/');
        _httpClient = new HttpClient();
        
        // Настройка Basic аутентификации
        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Получает меню с сервера
    /// </summary>
    /// <param name="withPrice">Включать ли цены в ответ</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список блюд</returns>
    /// <exception cref="ServingsApiException">Выбрасывается при ошибке API сервера или сетевой проблеме</exception>
    public async Task<List<MenuItem>> GetMenuAsync(bool withPrice = true, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetMenuRequest
            {
                CommandParameters = new GetMenuParameters { WithPrice = withPrice }
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/command", content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var menuResponse = JsonConvert.DeserializeObject<GetMenuResponse>(responseContent);

            if (menuResponse == null)
            {
                throw new ServingsApiException("Failed to deserialize server response");
            }

            if (!menuResponse.Success)
            {
                throw new ServingsApiException($"Server returned error: {menuResponse.ErrorMessage}", menuResponse.ErrorMessage);
            }

            return menuResponse.Data?.MenuItems ?? new List<MenuItem>();
        }
        catch (HttpRequestException ex)
        {
            throw new ServingsApiException("HTTP request failed", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ServingsApiException("Request timeout", ex);
        }
        catch (JsonException ex)
        {
            throw new ServingsApiException("JSON serialization/deserialization error", ex);
        }
    }

    /// <summary>
    /// Отправляет заказ на сервер
    /// </summary>
    /// <param name="order">Заказ для отправки</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <exception cref="ArgumentException">Выбрасывается при невалидных параметрах заказа</exception>
    /// <exception cref="ServingsApiException">Выбрасывается при ошибке API сервера или сетевой проблеме</exception>
    public async Task SendOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (string.IsNullOrWhiteSpace(order.OrderId))
        {
            throw new ArgumentException("OrderId is required", nameof(order));
        }

        if (order.MenuItems == null || !order.MenuItems.Any())
        {
            throw new ArgumentException("Order must contain at least one menu item", nameof(order));
        }

        try
        {
            var request = new SendOrderRequest
            {
                CommandParameters = new SendOrderParameters(order.OrderId, order.MenuItems)
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/command", content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var orderResponse = JsonConvert.DeserializeObject<SendOrderResponse>(responseContent);

            if (orderResponse == null)
            {
                throw new ServingsApiException("Failed to deserialize server response");
            }

            if (!orderResponse.Success)
            {
                throw new ServingsApiException($"Server returned error: {orderResponse.ErrorMessage}", orderResponse.ErrorMessage);
            }
        }
        catch (HttpRequestException ex)
        {
            throw new ServingsApiException("HTTP request failed", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ServingsApiException("Request timeout", ex);
        }
        catch (JsonException ex)
        {
            throw new ServingsApiException("JSON serialization/deserialization error", ex);
        }
    }

    
    public void Dispose()
    {
        if (_disposed) return;
        _httpClient?.Dispose();
        _disposed = true;
    }
}
