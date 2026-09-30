namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

internal partial class CA
{

    /// <summary>
    /// Return which contains indexes.
    /// </summary>
    internal static List<int> ReturnWhichContainsIndexes(string text, IList<string> terms)
    {
        var result = new List<int>();
        var i = 0;
        foreach (var term in terms)
        {
            if (text.Contains(term))
                result.Add(i);
            i++;
        }

        return result;
    }

    /// <summary>
    /// Starting with.
    /// </summary>
    internal static List<string> StartingWith(string prefix, List<string> list)
    {
        for (var i = list.Count - 1; i >= 0; i--)
            if (!list[i].StartsWith(prefix))
                list.RemoveAt(i);
        return list;
    }

    /// <summary>
    /// Postfix if not ending.
    /// </summary>
    internal static List<string> PostfixIfNotEnding(string prefix, List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            list[i] = prefix + list[i];
        return list;
    }

    /// <summary>
    /// Contains any from element bool.
    /// </summary>
    internal static bool ContainsAnyFromElementBool(string text, IList<string> list)
    {
        if (list.Count == 1 && list.First() == "*")
            return true;
        foreach (var item in list)
            if (text.Contains(item))
                return true;
        return false;
    }

    /// <summary>
    /// Trim.
    /// </summary>
    internal static List<string> Trim(List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            list[i] = list[i].Trim();
        return list;
    }

    /// <summary>
    /// Replace.
    /// </summary>
    private static string Replace(string text, string what, string replacement)
        => text.Replace(what, replacement);

    /// <summary>
    /// Replace.
    /// </summary>
    internal static void Replace(List<string> list, string what, string replacement)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = Replace(list[i], what, replacement);
        }
    }
}
