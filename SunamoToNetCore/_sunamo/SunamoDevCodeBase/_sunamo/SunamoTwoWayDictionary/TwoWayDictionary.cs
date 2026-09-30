namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoTwoWayDictionary;

internal class TwoWayDictionary<T, U> where T : notnull where U : notnull
{
    internal Dictionary<T, U> FirstToSecond { get; set; } = null!;
    internal Dictionary<U, T> SecondToFirst { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of TwoWayDictionary.
    /// </summary>
    internal TwoWayDictionary(int capacity)
    {
        FirstToSecond = new Dictionary<T, U>(capacity);
        SecondToFirst = new Dictionary<U, T>(capacity);
    }

    /// <summary>
    /// Initializes a new instance of TwoWayDictionary.
    /// </summary>
    internal TwoWayDictionary()
    {
        FirstToSecond = new Dictionary<T, U>();
        SecondToFirst = new Dictionary<U, T>();
    }

    /// <summary>
    /// Add.
    /// </summary>
    internal void Add(T key, U value)
    {
        FirstToSecond.Add(key, value);
        SecondToFirst.Add(value, key);
    }
}