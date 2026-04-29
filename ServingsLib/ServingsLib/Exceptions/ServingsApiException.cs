namespace ServingsLib.Exceptions;

/// <summary>
/// Ошибка работы с API сервера
/// </summary>
public class ServingsApiException : Exception
{
    public string? ErrorMessage { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="message"></param>
    public ServingsApiException(string message) : base(message)
    {
    }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="message"></param>
    /// <param name="errorMessage"></param>
    public ServingsApiException(string message, string? errorMessage) : base(message)
    {
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public ServingsApiException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
