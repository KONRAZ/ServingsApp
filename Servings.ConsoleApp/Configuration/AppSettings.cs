namespace Servings.ConsoleApp.Configuration;

/// <summary>
/// Настройки приложения.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// URL сервера Servings.
    /// </summary>
    public string ServerUrl { get; set; } = string.Empty;

    /// <summary>
    /// Имя пользователя для API.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Пароль для API.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Строка подключения к PostgreSQL.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
}
