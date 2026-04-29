namespace Servings.Application.DTOs;

/// <summary>
/// DTO для ответа на заказ.
/// </summary>
public class OrderResponseDto
{
    /// <summary>
    /// Успешность выполнения заказа.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Сообщение от сервера.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор заказа.
    /// </summary>
    public string? OrderId { get; set; }
}
