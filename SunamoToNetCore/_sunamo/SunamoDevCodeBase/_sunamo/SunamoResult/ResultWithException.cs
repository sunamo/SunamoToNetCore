namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoResult;

internal class ResultWithException<T>
{
    internal T Data { get; set; } = default!;
    internal string? Exc { get; set; }

    /// <summary>
    /// Initializes a new instance of ResultWithException.
    /// </summary>
    internal ResultWithException(T data)
    {
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of ResultWithException.
    /// </summary>
    internal ResultWithException(string exc)
    {
        this.Exc = exc;
    }

    /// <summary>
    /// Initializes a new instance of ResultWithException.
    /// </summary>
    internal ResultWithException(Exception exc)
    {
        this.Exc = exc.Message;
    }

    /// <summary>
    /// Initializes a new instance of ResultWithException.
    /// </summary>
    internal ResultWithException()
    {
    }
}