using Newtonsoft.Json;

namespace ServingsLib.Models;

/// <summary>
/// Представляет блюдо в меню ресторана
/// </summary>
public class MenuItem
{
    /// <summary>
    /// Уникальный идентификатор блюда
    /// </summary>
    [JsonProperty("Id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Артикул блюда
    /// </summary>
    [JsonProperty("Article")]
    public string Article { get; set; } = string.Empty;

    /// <summary>
    /// Наименование блюда
    /// </summary>
    [JsonProperty("Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Цена блюда
    /// </summary>
    [JsonProperty("Price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Является ли блюдо весовым товаром
    /// </summary>
    [JsonProperty("IsWeighted")]
    public bool IsWeighted { get; set; }

    /// <summary>
    /// Полный путь к блюду в меню
    /// </summary>
    [JsonProperty("FullPath")]
    public string FullPath { get; set; } = string.Empty;

    /// <summary>
    /// Список штрихкодов блюда
    /// </summary>
    [JsonProperty("Barcodes")]
    public List<string> Barcodes { get; set; } = new List<string>();
}
