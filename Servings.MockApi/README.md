# Servings Mock API

Простой мок-сервис для тестирования контрактов API Servings.

## Запуск

```bash
cd Servings.MockApi
dotnet run
```

Сервис запустится на `http://localhost:5001`

**Эндпоинт:** `POST http://localhost:5001/api/command`

## Поддерживаемые команды

### GetMenu

**Запрос:**
```json
{
  "Command": "GetMenu",
  "CommandParameters": {
    "WithPrice": true
  }
}
```

**Ответ:**
```json
{
  "Command": "GetMenu",
  "Success": true,
  "ErrorMessage": "",
  "Data": {
    "MenuItems": [
      {
        "Id": "5979224",
        "Article": "A1004292",
        "Name": "Каша гречневая",
        "Price": 50,
        "IsWeighted": false,
        "FullPath": "ПРОИЗВОДСТВО\\Гарниры",
        "Barcodes": ["57890975627974236429"]
      },
      {
        "Id": "9084246",
        "Article": "A1004293",
        "Name": "Конфеты Коровка",
        "Price": 300,
        "IsWeighted": true,
        "FullPath": "ДЕСЕРТЫ\\Развес",
        "Barcodes": []
      },
      {
        "Id": "1234567",
        "Article": "B1005678",
        "Name": "Салат Цезарь",
        "Price": 150,
        "IsWeighted": false,
        "FullPath": "САЛАТЫ\\Холодные",
        "Barcodes": ["4601234567890"]
      }
    ]
  }
}
```

### SendOrder

**Запрос:**
```json
{
  "Command": "SendOrder",
  "CommandParameters": {
    "OrderId": "62137983-1117-4D10-87C1-EF40A4348250",
    "MenuItems": [
      {
        "Id": "5979224",
        "Quantity": "1"
      },
      {
        "Id": "9084246",
        "Quantity": "0.408"
      }
    ]
  }
}
```

**Ответ:**
```json
{
  "Command": "SendOrder",
  "Success": true,
  "ErrorMessage": ""
}
```

## Тестирование

Для тестирования с основным приложением:

1. Запустите мок-API: `cd Servings.MockApi && dotnet run`
2. Измените `appsettings.json` в основном приложении:
   ```json
   {
     "ServerUrl": "http://localhost:5001"
   }
   ```
3. Запустите основное приложение

Мок-API будет логировать все полученные заказы в консоль.
