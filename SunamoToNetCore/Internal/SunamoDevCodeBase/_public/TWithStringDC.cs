namespace SunamoToNetCore.Internal._public;

public class TWithStringDC<T>
{
    public string path = null!;
    public T t = default!;

    public TWithStringDC()
    {
    }

    public TWithStringDC(T t, string path)
    {
        this.t = t;
        this.path = path;
    }

    public override string ToString() => path;
}
