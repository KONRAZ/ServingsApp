@echo off
REM Скрипт для запуска PostgreSQL и консольного приложения
REM Проверяет запущен ли контейнер PostgreSQL, если нет - запускает через docker-compose

echo === Запуск Servings Console App ===

REM Проверяем запущен ли контейнер servings-postgres
docker ps -q -f name=servings-postgres >nul 2>&1
if %errorlevel% equ 0 (
    echo ✅ Контейнер servings-postgres уже запущен
    set DB_READY=true
) else (
    echo 🔄 Контейнер servings-postgres не найден, запускаем...
    
    REM Запускаем docker-compose
    docker-compose up -d
    
    echo ⏳ Ожидание запуска PostgreSQL...
    docker-compose wait servings-postgres
    echo ✅ PostgreSQL готов к работе
)

REM Даем время на полную инициализацию
echo ⏳ Ожидание полной инициализации базы данных...
timeout /t 5 /nobreak >nul

REM Запускаем консольное приложение
echo 🚀 Запуск консольного приложения...
cd Servings.ConsoleApp
dotnet run

echo ✅ Приложение завершило работу
pause
