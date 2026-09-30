namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._public;

public class TWithStringDC<T>
{
    public string path = null!;
    public T t = default!;

    /// <summary>
    /// Initializes a new instance of TWithStringDC.
    /// </summary>
    public TWithStringDC()
    {
    }

    /// <summary>
    /// Initializes a new instance of TWithStringDC.
    /// </summary>
    public TWithStringDC(T t, string path)
    {
        this.t = t;
        this.path = path;
    }

    /// <summary>
    /// To string.
    /// </summary>
    public override string ToString() => path;
}