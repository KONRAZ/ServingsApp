namespace Servings.Infrastructure.Logging;

/// <summary>
/// Интерфейс для логирования в файл.
/// </summary>
public interface IFileLogger
{
    /// <summary>
    /// Записать информационное сообщение в лог.
    /// </summary>
    Task LogInfoAsync(string message);

    /// <summary>
    /// Записать сообщение об ошибке в лог.
    /// </summary>
    Task LogErrorAsync(string message, Exception? exception = null);

    /// <summary>
    /// Записать предупреждение в лог.
    /// </summary>
    Task LogWarningAsync(string message);

    /// <summary>
    /// Записать отладочное сообщение в лог.
    /// </summary>
    Task LogDebugAsync(string message);
}
