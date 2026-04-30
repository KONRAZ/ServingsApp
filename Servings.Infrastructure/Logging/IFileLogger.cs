namespace Servings.Infrastructure.Logging;

/// <summary>
/// Интерфейс для логирования в файл.
/// </summary>
public interface IFileLogger
{
    /// <summary>
    /// Записать информационное сообщение в лог.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task LogInfoAsync(string message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Записать сообщение об ошибке в лог.
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    /// <param name="exception">Исключение (опционально)</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task LogErrorAsync(string message, Exception? exception = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Записать предупреждение в лог.
    /// </summary>
    /// <param name="message">Предупреждение</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task LogWarningAsync(string message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Записать отладочное сообщение в лог.
    /// </summary>
    /// <param name="message">Отладочное сообщение</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task LogDebugAsync(string message, CancellationToken cancellationToken = default);
}
