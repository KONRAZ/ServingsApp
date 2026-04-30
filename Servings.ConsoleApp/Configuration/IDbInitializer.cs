namespace Servings.ConsoleApp.Configuration;

/// <summary>
/// Интерфейс для инициализации базы данных.
/// </summary>
public interface IDbInitializer
{
    /// <summary>
    /// Инициализировать базу данных.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
