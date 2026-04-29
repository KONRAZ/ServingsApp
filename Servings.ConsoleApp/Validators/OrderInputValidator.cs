namespace Servings.ConsoleApp.Validators;

/// <summary>
/// Валидатор для пользовательского ввода заказов.
/// </summary>
public static class OrderInputValidator
{
    /// <summary>
    /// Распарсить строку ввода заказа в формате "Код1:Количество1;Код2:Количество2".
    /// </summary>
    public static List<(string article, int quantity)>? ParseOrderInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        try
        {
            var items = new List<(string, int)>();
            var pairs = input.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var parts = pair.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2) return null;

                var article = parts[0].Trim();
                if (!int.TryParse(parts[1].Trim(), out int quantity) || quantity <= 0)
                    return null;

                items.Add((article, quantity));
            }

            return items.Any() ? items : null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Валидировать список позиций заказа.
    /// </summary>
    public static List<string> ValidateOrderItems(List<(string article, int quantity)> items, HashSet<string> validArticles)
    {
        var errors = new List<string>();

        foreach (var (article, quantity) in items)
        {
            // Валидация артикула
            if (string.IsNullOrWhiteSpace(article) || article.Length < 2 || article.Length > 50)
            {
                errors.Add($"Артикул '{article}' должен содержать от 2 до 50 символов");
                continue;
            }

            // Проверка существования артикула в меню
            if (!validArticles.Contains(article))
            {
                errors.Add($"Блюдо с кодом '{article}' не найдено в меню");
            }

            // Валидация количества
            if (quantity <= 0 || quantity > 100)
            {
                errors.Add($"Количество для блюда '{article}' должно быть числом от 1 до 100");
            }
        }

        return errors;
    }

    /// <summary>
    /// Получить сообщение об ошибке для неверного формата ввода.
    /// </summary>
    public static string GetInputFormatErrorMessage()
    {
        return "Формат ввода: 'Код1:Количество1;Код2:Количество2' (например: 'B001:2;C003:1')";
    }
}
