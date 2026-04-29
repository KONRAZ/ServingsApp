using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/api/command", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    
    try
    {
        var request = JsonDocument.Parse(body);
        var command = request.RootElement.GetProperty("Command").GetString();
        
        switch (command)
        {
            case "GetMenu":
                await HandleGetMenu(context);
                break;
            case "SendOrder":
                await HandleSendOrder(context, request);
                break;
            default:
                await HandleUnknownCommand(context, command ?? "Unknown");
                break;
        }
    }
    catch (Exception ex)
    {
        await HandleError(context, ex);
    }
});

app.Run("http://0.0.0.0:80");

async Task HandleGetMenu(HttpContext context)
{
    var response = new
    {
        Command = "GetMenu",
        Success = true,
        ErrorMessage = "",
        Data = new
        {
            MenuItems = new[]
            {
                new
                {
                    Id = "5979224",
                    Article = "A1004292",
                    Name = "Каша гречневая",
                    Price = 50,
                    IsWeighted = false,
                    FullPath = "ПРОИЗВОДСТВО\\Гарниры",
                    Barcodes = new[] { "57890975627974236429" }
                },
                new
                {
                    Id = "9084246",
                    Article = "A1004293",
                    Name = "Конфеты Коровка",
                    Price = 300,
                    IsWeighted = true,
                    FullPath = "ДЕСЕРТЫ\\Развес",
                    Barcodes = new string[0]
                },
                new
                {
                    Id = "1234567",
                    Article = "B1005678",
                    Name = "Салат Цезарь",
                    Price = 150,
                    IsWeighted = false,
                    FullPath = "САЛАТЫ\\Холодные",
                    Barcodes = new[] { "4601234567890" }
                }
            }
        }
    };
    
    context.Response.StatusCode = 200;
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
}

async Task HandleSendOrder(HttpContext context, JsonDocument request)
{
    try
    {
        var commandParams = request.RootElement.GetProperty("CommandParameters");
        var orderId = commandParams.GetProperty("OrderId").GetString();
        var menuItems = commandParams.GetProperty("MenuItems");
        
        Console.WriteLine($"Received order {orderId} with {menuItems.GetArrayLength()} items:");
        
        foreach (var item in menuItems.EnumerateArray())
        {
            var id = item.GetProperty("Id").GetString();
            var quantity = item.GetProperty("Quantity").GetString();
            Console.WriteLine($"  - Item {id}: {quantity}");
        }
        
        var response = new
        {
            Command = "SendOrder",
            Success = true,
            ErrorMessage = ""
        };
        
        context.Response.StatusCode = 200;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    catch (Exception ex)
    {
        var response = new
        {
            Command = "SendOrder",
            Success = false,
            ErrorMessage = $"Error processing order: {ex.Message}"
        };
        
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

async Task HandleUnknownCommand(HttpContext context, string command)
{
    var response = new
    {
        Command = command,
        Success = false,
        ErrorMessage = $"Unknown command: {command}"
    };
    
    context.Response.StatusCode = 400;
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
}

async Task HandleError(HttpContext context, Exception ex)
{
    var response = new
    {
        Command = "Error",
        Success = false,
        ErrorMessage = $"Internal server error: {ex.Message}"
    };
    
    context.Response.StatusCode = 500;
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
}
