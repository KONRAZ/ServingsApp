using Servings.Infrastructure.DTOs;

namespace Servings.Infrastructure.External;

/// <summary>
/// Адаптер для работы с внешним API ServingsLib.
/// </summary>
public class ServingsApiClientWrapper : IServingsApiClient
{
    private readonly ServingsLib.ServingsClient _client;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="serverUrl"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public ServingsApiClientWrapper(string serverUrl, string username, string password)
    {
        _client = new ServingsLib.ServingsClient(serverUrl, username, password);
    }

    /// <inheritdoc/>
    public async Task<List<MenuItemDto>> GetMenuAsync()
    {
        try
        {
            var menuItems = await _client.GetMenuAsync();
            
            return menuItems.Select(item => new MenuItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Article = item.Article,
                Price = item.Price
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to get menu: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<OrderResponseDto> SendOrderAsync(CreateOrderRequestDto request)
    {
        try
        {
            var orderItems = request.Items.Select(item => new ServingsLib.Models.OrderItem
            {
                Id = item.Id,
                Quantity = item.Quantity.ToString()
            }).ToList();

            var order = new ServingsLib.Models.Order(Guid.NewGuid().ToString(), orderItems);
            await _client.SendOrderAsync(order);

            return new OrderResponseDto
            {
                Success = true,
                Message = "Order sent successfully",
                OrderId = order.OrderId
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to send order: {ex.Message}", ex);
        }
    }
}
