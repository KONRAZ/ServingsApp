namespace Servings.Infrastructure.Logging;

/// <summary>
/// Реализация файлового логгера.
/// </summary>
public class FileLogger : IFileLogger
{
    private readonly string _logDirectory;
    private readonly string _logFileName;

    /// <summary>
    /// .ctor
    /// </summary>
    public FileLogger()
    {
        _logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        _logFileName = $"test-sms-console-app-{DateTime.Now:yyyyMMdd}.log";
        
        // Создаем директорию для логов, если она не существует
        if (!Directory.Exists(_logDirectory))
        {
            Directory.CreateDirectory(_logDirectory);
        }
    }

    /// <inheritdoc/>
    public async Task LogInfoAsync(string message)
    {
        await WriteLogAsync("INFO", message);
    }

    /// <inheritdoc/>
    public async Task LogErrorAsync(string message, Exception? exception = null)
    {
        await WriteLogAsync("ERROR", message, exception);
    }

    /// <inheritdoc/>
    public async Task LogWarningAsync(string message)
    {
        await WriteLogAsync("WARNING", message);
    }

    /// <inheritdoc/>
    public async Task LogDebugAsync(string message)
    {
        await WriteLogAsync("DEBUG", message);
    }

    private string GetLogFilePath()
    {
        return Path.Combine(_logDirectory, _logFileName);
    }

    private async Task WriteLogAsync(string level, string message, Exception? exception = null)
    {
        var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
        
        if (exception != null)
        {
            logEntry += $" | Exception: {exception.Message}";
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                logEntry += $"\nStack Trace: {exception.StackTrace}";
            }
        }

        logEntry += Environment.NewLine;

        var logFilePath = GetLogFilePath();
        await File.AppendAllTextAsync(logFilePath, logEntry);
    }
}
