namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._public;

/// <summary>
/// Represents the t with string DC t type.
/// </summary>
public class TWithStringDC<T>
{
    /// <summary>
    /// The path.
    /// </summary>
    public string path = null!;
    /// <summary>
    /// The t.
    /// </summary>
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
    public TWithStringDC(T item, string path)
    {
        this.t = item;
        this.path = path;
    }

    /// <summary>
    /// To string.
    /// </summary>
    public override string ToString() => path;
}