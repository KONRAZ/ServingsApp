# ServingsLib

Библиотека для обработки запросов к серверу заказов с поддержкой HTTP и gRPC протоколов.

## Возможности

- **Два протокола**: HTTP с Basic аутентификацией и gRPC
- Получение меню с сервера
- Отправка заказов на сервер
- Современная асинхронная архитектура
- Обработка ошибок с кастомными исключениями
- Поддержка .NET 8.0 и .NET 10.0
- Multi-targeting для разных версий .NET

## Установка

Библиотека компилируется в DLL файл и может быть добавлена как зависимость в другие проекты.

## Выбор протокола

Библиотека предоставляет две версии для разных протоколов:

### 1. ServingsLib (HTTP с Basic аутентификацией)
- Традиционный HTTP/JSON протокол
- Basic аутентификация
- Совместимость с существующими серверами

### 2. ServingsLibgRPC (gRPC без авторизации)
- Современный gRPC протокол
- Protobuf сериализация
- Высокая производительность
- Без авторизации (согласно требованиям)

## Использование (HTTP версия)

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

## Использование (gRPC версия)

### Инициализация клиента

```csharp
using ServingsLib;

// Создание gRPC клиента (без авторизации)
var client = new ServingsGrpcClient("https://localhost:5001");
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
catch (ServingsGrpcException ex)
{
    Console.WriteLine($"Ошибка gRPC: {ex.Message}");
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
    // Создание элементов заказа (используем gRPC модели напрямую)
    var orderItems = new List<OrderItem>
    {
        new OrderItem { Id = "5979224", Quantity = 1.0 },      // 1 порция каши
        new OrderItem { Id = "9084246", Quantity = 0.408 }   // 0.408 кг конфет
    };
    
    // Создание заказа
    var order = new Order
    {
        Id = "62137983-1117-4D10-87C1-EF40A4348250"
    };
    order.OrderItems.AddRange(orderItems);
    
    // Отправка заказа на сервер
    await client.SendOrderAsync(order);
    
    Console.WriteLine("Заказ успешно отправлен!");
}
catch (ServingsGrpcException ex)
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
using var client = new ServingsGrpcClient("https://localhost:5001");
var menu = await client.GetMenuAsync();
// Ресурсы будут автоматически освобождены
```

## Модели данных

### Различия между версиями

| Свойство | HTTP версия | gRPC версия |
|----------|-------------|-------------|
| **Price** | `decimal` | `double` |
| **Quantity** | `string` | `double` |
| **Namespace** | `ServingsLib.Models` | `Sms.Test` |
| **Авторизация** | Basic Auth | Без авторизации |

### MenuItem (общие свойства)
- `Id` - Уникальный идентификатор блюда
- `Article` - Артикул
- `Name` - Наименование
- `Price` - Цена (`decimal` в HTTP, `double` в gRPC)
- `IsWeighted` - Является ли весовым товаром
- `FullPath` - Полный путь в меню
- `Barcodes` - Список штрихкодов

### Order
- **HTTP**: `OrderId` (string), `MenuItems` (List<OrderItem>)
- **gRPC**: `Id` (string), `OrderItems` (RepeatedField<OrderItem>)

### OrderItem
- **HTTP**: `Id` (string), `Quantity` (string)
- **gRPC**: `Id` (string), `Quantity` (double)

## Обработка ошибок

### HTTP версия
- `ServingsApiException` - Ошибки API сервера или сетевые проблемы
- `ArgumentException` - Ошибки валидации входных параметров
- `ArgumentNullException` - Пустые или null параметры

### gRPC версия
- `ServingsGrpcException` - Ошибки gRPC сервера или сетевые проблемы
- `ArgumentException` - Ошибки валидации входных параметров
- `ArgumentNullException` - Пустые или null параметры
- `RpcException` - Низкоуровневые ошибки gRPC (оборачиваются в ServingsGrpcException)

## Требования

### Общие требования
- .NET 8.0 или .NET 10.0

### HTTP версия (ServingsLib)
- Newtonsoft.Json 13.0.3

### gRPC версия (ServingsLibgRPC)
- Grpc.Net.Client 2.59.0
- Google.Protobuf 3.25.1
- Grpc.Tools 2.59.0 (для генерации кода из proto)

## Компиляция

```bash
# Компиляция всех проектов
dotnet build

# Компиляция только HTTP версии
dotnet build ServingsLib/ServingsLib.csproj

# Компиляция только gRPC версии
dotnet build ServingsLibgRPC/ServingsLibgRPC.csproj
```

## Тестирование

```bash
# Запуск всех тестов
dotnet test

# Запуск HTTP тестов
dotnet test Tests.ServingsLib/Tests.ServingsLib.csproj

# Запуск gRPC тестов
dotnet test Tests.ServingsLibgRPC/Tests.ServingsLibgRPC.csproj
```

## Структура проекта

```
ServingsLib/
├── ServingsLib/                    # HTTP библиотека
│   ├── Models/                     # Доменные модели
│   ├── Requests/                   # HTTP запросы
│   ├── Responses/                  # HTTP ответы
│   ├── Services/                   # HTTP клиент
│   └── ServingsClient.cs           # Публичный фасад
├── ServingsLibgRPC/                # gRPC библиотека
│   ├── Protos/
│   │   └── sms_test.proto         # gRPC определение сервиса
│   ├── Services/
│   │   └── SmsTestGrpcClient.cs   # gRPC клиент
│   ├── Exceptions/
│   │   └── ServingsGrpcException.cs
│   └── ServingsGrpcClient.cs       # Публичный фасад
├── Tests.ServingsLib/              # HTTP тесты
│   ├── Models/                     # Тесты моделей
│   ├── Services/                   # Тесты HTTP клиента
│   └── SerializationTests.cs       # Тесты сериализации
└── Tests.ServingsLibgRPC/          # gRPC тесты
    ├── Services/
    │   ├── SmsTestGrpcClientTests.cs
    │   └── ServingsGrpcClientTests.cs
    └── ...                         # Другие gRPC тесты
```

## Версии и совместимость

- **ServingsLib 1.0.0** - HTTP версия с Basic аутентификацией
- **ServingsLibgRPC 1.0.0** - gRPC версия без авторизации
- **Multi-targeting**: .NET 8.0 и .NET 10.0
- **Backward compatibility**: Обратная совместимость сохраняется в рамках каждой версии
