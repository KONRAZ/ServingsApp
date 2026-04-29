# ServingsLib

Библиотека для обработки запросов к серверу заказов с HTTP Basic аутентификацией.

## Возможности

- Получение меню с сервера
- Отправка заказов на сервер
- Современная асинхронная архитектура
- Обработка ошибок с кастомными исключениями
- Поддержка .NET 8.0 и .NET 10.0

## Установка

Библиотека компилируется в DLL файл и может быть добавлена как зависимость в другие проекты.

## Использование

### Инициализация клиента

```csharp
using ServingsLib;

// Создание клиента с учетными данными
var client = new ServingsClient("https://your-server.com", "username", "password");
```

### Получение меню

```csharp
try
{
    // Получение меню с ценами
    var menuItems = await client.GetMenuAsync(withPrice: true);
    
    foreach (var item in menuItems)
    {
        Console.WriteLine($"{item.Name} - {item.Price} руб.");
        Console.WriteLine($"Артикул: {item.Article}");
        Console.WriteLine($"Путь: {item.FullPath}");
        Console.WriteLine($"Весовой товар: {item.IsWeighted}");
        Console.WriteLine();
    }
}
catch (ServingsApiException ex)
{
    Console.WriteLine($"Ошибка API: {ex.Message}");
    if (!string.IsNullOrEmpty(ex.ErrorMessage))
    {
        Console.WriteLine($"Детали ошибки: {ex.ErrorMessage}");
    }
}
```

### Создание и отправка заказа

```csharp
try
{
    // Создание элементов заказа
    var orderItems = new List<OrderItem>
    {
        ServingsClient.CreateOrderItem("5979224", "1"),      // 1 порция каши
        ServingsClient.CreateOrderItem("9084246", "0.408")   // 0.408 кг конфет
    };
    
    // Создание заказа
    var order = ServingsClient.CreateOrder(
        "62137983-1117-4D10-87C1-EF40A4348250", 
        orderItems
    );
    
    // Отправка заказа на сервер
    await client.SendOrderAsync(order);
    
    Console.WriteLine("Заказ успешно отправлен!");
}
catch (ServingsApiException ex)
{
    Console.WriteLine($"Ошибка при отправке заказа: {ex.Message}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Ошибка валидации: {ex.Message}");
}
```

### Использование с using для автоматической очистки ресурсов

```csharp
using var client = new ServingsClient("https://your-server.com", "username", "password");
var menu = await client.GetMenuAsync();
// Ресурсы будут автоматически освобождены
```

## Модели данных

### MenuItem
- `Id` - Уникальный идентификатор блюда
- `Article` - Артикул
- `Name` - Наименование
- `Price` - Цена
- `IsWeighted` - Является ли весовым товаром
- `FullPath` - Полный путь в меню
- `Barcodes` - Список штрихкодов

### Order
- `OrderId` - Уникальный идентификатор заказа
- `MenuItems` - Список элементов заказа

### OrderItem
- `Id` - ID блюда из меню
- `Quantity` - Количество (в виде строки)

## Обработка ошибок

Библиотека использует следующие типы исключений:

- `ServingsApiException` - Ошибки API сервера или сетевые проблемы
- `ArgumentException` - Ошибки валидации входных параметров
- `ArgumentNullException` - Пустые или null параметры

## Требования

- .NET 8.0 или .NET 10.0
- Newtonsoft.Json 13.0.3

## Компиляция

```bash
dotnet build
```
