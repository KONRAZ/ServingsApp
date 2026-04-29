namespace Servings.ConsoleApp.Configuration;

/// <summary>
/// Интерфейс для инициализации базы данных.
/// </summary>
public interface IDbInitializer
{
    /// <summary>
    /// Инициализировать базу данных.
    /// </summary>
    Task InitializeAsync();
}
