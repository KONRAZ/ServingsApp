namespace ServingsLib.Exceptions;

public class ServingsApiException : Exception
{
    public string? ErrorMessage { get; }

    public ServingsApiException(string message) : base(message)
    {
    }

    public ServingsApiException(string message, string? errorMessage) : base(message)
    {
        ErrorMessage = errorMessage;
    }

    public ServingsApiException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
