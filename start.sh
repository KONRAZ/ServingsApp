#!/bin/bash

# Скрипт для запуска PostgreSQL и консольного приложения
# Проверяет запущен ли контейнер PostgreSQL, если нет - запускает через docker-compose

echo "=== Запуск Servings Console App ==="

# Проверяем запущен ли контейнер servings-postgres
if [ "$(docker ps -q -f name=servings-postgres)" ]; then
    echo "✅ Контейнер servings-postgres уже запущен"
    DB_READY=true
else
    echo "🔄 Контейнер servings-postgres не найден, запускаем..."
    
    # Запускаем docker-compose
    docker-compose up -d
    
    echo "⏳ Ожидание запуска PostgreSQL..."
    docker-compose wait servings-postgres
    echo "✅ PostgreSQL готов к работе"
fi

# Даем время на полную инициализацию
echo "⏳ Ожидание полной инициализации базы данных..."
sleep 5

# Запускаем консольное приложение
echo "🚀 Запуск консольного приложения..."
cd Servings.ConsoleApp
dotnet run

echo "✅ Приложение завершило работу"
