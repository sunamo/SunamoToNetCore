namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoExtensions;

internal static class ListExtensions
{
    // Direct edit
    /// <summary>
    /// Leading range.
    /// </summary>
    internal static List<string> LeadingRange(this List<string> list, IList<string> items)
    {
        for (var i = items.Count - 1; i >= 0; i--) list.Insert(0, items[i]);
        return list;
    }
}