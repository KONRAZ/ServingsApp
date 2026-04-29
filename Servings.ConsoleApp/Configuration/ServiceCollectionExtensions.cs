using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Servings.Infrastructure.Data;
using Servings.Infrastructure.Data.Repositories;
using Servings.Infrastructure.External;
using Servings.Infrastructure.Logging;
using Servings.Application.Services;
using Servings.Application.Interfaces;
using Servings.Infrastructure.Data.Repositories.Interfaces;

namespace Servings.ConsoleApp.Configuration;

/// <summary>
/// Расширения для коллекции сервисов.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить сервисы приложения.
    /// </summary>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Настройки
        services.Configure<AppSettings>(configuration);

        // Infrastructure
        services.AddDbContext<ApplicationContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString()));

        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IFileLogger, FileLogger>();
        services.AddScoped<IServingsApiClient, ServingsApiClientWrapper>(provider =>
        {
            var settings = provider.GetRequiredService<AppSettings>();
            return new ServingsApiClientWrapper(
                settings.ServerUrl, 
                settings.Username, 
                settings.Password);
        });

        // Application
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IOrderService, OrderService>();

        // Добавляем сервис для автоматической миграции
        services.AddScoped<IDbInitializer, DbInitializer>();

        return services;
    }

    /// <summary>
    /// Получить строку подключения.
    /// </summary>
    private static string GetConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("ServingsDbConnection") 
               ?? configuration["ConnectionString"] 
               ?? throw new InvalidOperationException("Connection string not found");
    }
}
