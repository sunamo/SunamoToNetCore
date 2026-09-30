namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._public;

internal class ResultWithExceptionDC<T>
{
    public T Data { get; set; } = default!;

    public string Exc { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of ResultWithExceptionDC.
    /// </summary>
    public ResultWithExceptionDC(T data)
    {
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of ResultWithExceptionDC.
    /// </summary>
    public ResultWithExceptionDC(string exc)
    {
        this.Exc = exc;
    }

    /// <summary>
    /// Initializes a new instance of ResultWithExceptionDC.
    /// </summary>
    public ResultWithExceptionDC(Exception exc)
    {
        this.Exc = Exceptions.TextOfExceptions(exc);
    }

    /// <summary>
    /// Initializes a new instance of ResultWithExceptionDC.
    /// </summary>
    public ResultWithExceptionDC()
    {
    }
}