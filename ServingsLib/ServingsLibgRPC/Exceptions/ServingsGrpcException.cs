namespace ServingsLib.Exceptions;

/// <summary>
/// Ошибка работы с gRPC API сервера
/// </summary>
public class ServingsGrpcException : Exception
{
    public string? ErrorMessage { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="message"></param>
    public ServingsGrpcException(string message) : base(message)
    {
    }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="message"></param>
    /// <param name="errorMessage"></param>
    public ServingsGrpcException(string message, string? errorMessage) : base(message)
    {
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public ServingsGrpcException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
