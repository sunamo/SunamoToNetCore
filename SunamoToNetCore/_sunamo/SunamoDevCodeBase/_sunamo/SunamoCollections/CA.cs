namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoCollections;

internal partial class CA
{

    /// <summary>
    /// Return which contains indexes.
    /// </summary>
    internal static List<int> ReturnWhichContainsIndexes(string text, IList<string> terms)
    {
        var result = new List<int>();
        var index = 0;
        foreach (var term in terms)
        {
            if (text.Contains(term))
                result.Add(index);
            index++;
        }

        return result;
    }

    /// <summary>
    /// Starting with.
    /// </summary>
    internal static List<string> StartingWith(string prefix, List<string> list)
    {
        for (var index = list.Count - 1; index >= 0; index--)
            if (!list[index].StartsWith(prefix))
                list.RemoveAt(index);
        return list;
    }

    /// <summary>
    /// Postfix if not ending.
    /// </summary>
    internal static List<string> PostfixIfNotEnding(string prefix, List<string> list)
    {
        for (var index = 0; index < list.Count; index++)
            list[index] = prefix + list[index];
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
        for (var index = 0; index < list.Count; index++)
            list[index] = list[index].Trim();
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
        for (int index = 0; index < list.Count; index++)
        {
            list[index] = Replace(list[index], what, replacement);
        }
    }
}