# Запуск Servings Console App

## Автоматический запуск с PostgreSQL и Mock API

### Для Windows
```cmd
start.bat
```

### Для Linux/macOS
```bash
chmod +x start.sh
./start.sh
```

**По умолчанию консольное приложение подключается к мок-сервису на `http://localhost:5001`**

**Конфигурации консольного приложения (адрес сервера заказов, строка подключения к базе) находится в `.\ServingsApp\Servings.ConsoleApp\appsettings.json`**

## Что делают скрипты

1. **Проверяют запущен ли контейнер** `servings-postgres`
2. **Если контейнер не запущен:**
   - Запускают `docker-compose up -d`
   - Запускают PostgreSQL и мок-сервис
   - Docker автоматически создает схему `servings`
   - Ждут готовности PostgreSQL через `docker-compose wait`
   - Запускают приложение только когда база полностью готова
3. **Запускают консольное приложение** `dotnet run`
   - EF Core автоматически накатывает миграции и создает таблицы
   - Приложение подключается к мок-сервису для тестирования

## Настройки базы данных

**PostgreSQL:**
- Хост: localhost:5432
- База: servings_db
- Пользователь: postgres
- Пароль: servingspass
- Схема: servings (создается автоматически в Docker)

**Docker Compose:**
- Контейнеры: servings-postgres, servings-mockapi
- Порты: 5432 (PostgreSQL), 5001 (Mock API)
- Volume: postgres_data (персистентные данные)
- Health check: готовность PostgreSQL
- Автоматическое создание схемы servings
- Зависимость: mock API зависит от PostgreSQL

## Ручной запуск

### 1. Запуск PostgreSQL
```bash
docker-compose up -d
```

### 2. Проверка готовности
```bash
docker exec servings-postgres pg_isready -U postgres -d servings_db
```

### 3. Запуск приложения
```bash
cd Servings.ConsoleApp
dotnet run
```

## Остановка контейнеров

### Остановка всех контейнеров
```bash
docker-compose down
```
Останавливает и удаляет контейнеры:
- servings-postgres (PostgreSQL)
- servings-mockapi (Mock API)

### Полная очистка (включая данные)
```bash
docker-compose down -v
```
Дополнительно удаляет volume postgres_data с данными.

### Проверка статуса контейнеров
```bash
docker-compose ps
```

### Просмотр логов
```bash
# Логи PostgreSQL
docker-compose logs servings-postgres

# Логи Mock API
docker-compose logs servings-mockapi

# Логи всех сервисов
docker-compose logs
```

## Тестовые данные

Тестовые блюда загружаются из API при первом запуске приложения:
- A1004292 - Каша гречневая (50 руб)
- A1004293 - Конфеты Коровка (300 руб)
- B1005678 - Салат Цезарь (150 руб)

**Таблицы создаются автоматически через EF Core миграции при первом запуске приложения.**

## Требования

- Docker и Docker Compose
- .NET 8.0 SDK
- PowerShell (для Windows) или Bash (для Linux/macOS)
