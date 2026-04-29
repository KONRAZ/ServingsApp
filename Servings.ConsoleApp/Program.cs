using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Servings.ConsoleApp.Configuration;
using Servings.ConsoleApp.Validators;
using Servings.Application.Interfaces;
using Servings.Infrastructure.Logging;
using Servings.Infrastructure.DTOs;

namespace Servings.ConsoleApp;

class Program
{
    private static IServiceProvider? _serviceProvider;
    private static IMenuService? _menuService;
    private static IOrderService? _orderService;
    private static IFileLogger? _logger;

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== СЕРВИС ЗАКАЗОВ ===");
        Console.WriteLine();

        try
        {
            var host = CreateHostBuilder(args).Build();
            _serviceProvider = host.Services;
            
            // Инициализируем базу данных
            var dbInitializer = _serviceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
            
            _menuService = _serviceProvider.GetRequiredService<IMenuService>();
            _orderService = _serviceProvider.GetRequiredService<IOrderService>();
            _logger = _serviceProvider.GetRequiredService<IFileLogger>();

            await _logger.LogInfoAsync("Приложение запущено");
            await RunApplicationAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка: {ex.Message}");
            if (_logger != null)
            {
                await _logger.LogErrorAsync("Критическая ошибка приложения", ex);
            }
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddApplicationServices(context.Configuration);
            });
    }

    private static async Task RunApplicationAsync()
    {
        // Шаг 1: Автоматическая загрузка меню с сервера
        Console.WriteLine("\nЗагрузка меню с сервера...");
        var loadSuccess = await _menuService!.LoadMenuFromServerAsync();
        
        if (!loadSuccess)
        {
            Console.WriteLine("❌ Ошибка при загрузке меню с сервера.");
            Console.WriteLine("Приложение прекратило работу из-за критической ошибки.");
            await _logger!.LogErrorAsync("Критическая ошибка: не удалось загрузить меню с сервера");
            
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
            return;
        }

        // Шаг 2: Автоматическое отображение загруженного меню
        Console.WriteLine("✅ Меню успешно загружено");
        await _menuService!.DisplayMenuAsync();
        await _logger!.LogInfoAsync("Меню успешно загружено и отображено");

        // Шаг 3: Основной цикл работы с заказами
        await ProcessOrdersAsync();
    }

    private static async Task ProcessOrdersAsync()
    {
        while (true)
        {
            Console.WriteLine("\n=== РАБОТА С ЗАКАЗАМИ ===");
            Console.WriteLine("1. Создать новый заказ");
            Console.WriteLine("2. Выход");
            Console.WriteLine("========================");

            Console.Write("\nВыберите действие: ");
            var choice = Console.ReadLine()?.Trim().ToUpper();

            switch (choice)
            {
                case "1":
                    await CreateAndProcessOrderAsync();
                    break;
                case "2":
                case "Q":
                    await _logger!.LogInfoAsync("Пользователь вышел из приложения");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    private static async Task CreateAndProcessOrderAsync()
    {
        // Создаем новый заказ
        var order = await _orderService!.CreateOrderAsync();
        Console.WriteLine("✅ Создан новый заказ");
        await _logger!.LogInfoAsync("Создан новый заказ");

        // Ввод списка блюд
        var orderItems = await GetOrderItemsFromUser();
        if (orderItems == null) return; // Пользователь отменил ввод

        // Добавляем позиции в заказ
        foreach (var (article, quantity) in orderItems)
        {
            var success = await _orderService!.AddItemToOrderAsync(order, article, quantity);
            if (!success)
            {
                Console.WriteLine($"❌ Не удалось добавить позицию {article} в заказ");
                await _logger!.LogWarningAsync($"Не удалось добавить позицию {article} в заказ");
            }
        }

        // Отправляем заказ на сервер
        await SendOrderAndShowResult(order);
    }

    private static async Task<List<(string article, int quantity)>?> GetOrderItemsFromUser()
    {
        while (true)
        {
            Console.Write("\nВведите список блюд (формат: Код1:Количество1;Код2:Количество2): ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ввод отменен.");
                return null;
            }

            var items = OrderInputValidator.ParseOrderInput(input);
            if (items == null)
            {
                Console.WriteLine($"❌ {OrderInputValidator.GetInputFormatErrorMessage()}");
                continue;
            }

            // Проверяем существование всех кодов и корректность количества
            var menuItems = await _menuService!.GetLocalMenuAsync();
            var validArticles = menuItems.Select(m => m.Article).ToHashSet();
            
            var validationErrors = OrderInputValidator.ValidateOrderItems(items, validArticles);
            if (validationErrors.Any())
            {
                Console.WriteLine("❌ Обнаружены ошибки:");
                foreach (var error in validationErrors)
                {
                    Console.WriteLine($"   - {error}");
                }
                Console.WriteLine("Попробуйте снова.");
                continue;
            }

            return items;
        }
    }

    private static async Task SendOrderAndShowResult(CreateOrderRequestDto order)
    {
        if (!order.Items.Any())
        {
            Console.WriteLine("Заказ пуст. Нечего отправлять.");
            return;
        }

        Console.WriteLine("\nОтправка заказа на сервер...");
        var response = await _orderService!.SendOrderAsync(order);

        if (response.Success)
        {
            Console.WriteLine("УСПЕХ");
            Console.WriteLine($"ID заказа на сервере: {response.OrderId}");
            await _logger!.LogInfoAsync($"Заказ успешно отправлен. Server OrderId: {response.OrderId}");
        }
        else
        {
            Console.WriteLine($"ОШИБКА: {response.Message}");
            await _logger!.LogWarningAsync($"Ошибка при отправке заказа: {response.Message}");
        }
    }
}
