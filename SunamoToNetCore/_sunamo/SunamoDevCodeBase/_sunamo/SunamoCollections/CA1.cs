namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoCollections;

internal partial class CA
{

    /// <summary>
    /// Join i list.
    /// </summary>
    internal static List<T> JoinIList<T>(params IList<T>[] lists)
    {
        var result = new List<T>();
        foreach (var list in lists)
        {
            foreach (var element in list)
            {
                result.Add((T)element);
            }
        }

        return result;
    }

    /// <summary>
    /// Ensure backslash.
    /// </summary>
    internal static List<string> EnsureBackslash(List<string> paths)
    {
        for (int index = 0; index < paths.Count; index++)
        {
            string path = paths[index];
            if (path[path.Length - 1] != '\\')
            {
                paths[index] = path + "\\";
            }
        }

        return paths;
    }

    /// <summary>
    /// Contains element.
    /// </summary>
    internal static bool ContainsElement<T>(IList<T> list, T element)
    {
        if (list.Count == 0)
        {
            return false;
        }

        foreach (T item in list)
        {
            if (Comparer<T>.Equals(item, element))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Remove wildcard.
    /// </summary>
    internal static void RemoveWildcard(List<string> list, string mask)
    {
        //https://stackoverflow.com/a/15275806
        for (int index = list.Count - 1; index >= 0; index--)
        {
            if (SH.MatchWildcard(list[index], mask))
            {
                list.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// Prepend.
    /// </summary>
    internal static List<string> Prepend(string prefix, List<string> list)
    {
        for (int index = 0; index < list.Count; index++)
        {
            if (!list[index].StartsWith(prefix))
            {
                list[index] = prefix + list[index];
            }
        }

        return list;
    }

    /// <summary>
    /// To list string.
    /// </summary>
    internal static List<string> ToListString(params string[] values)
        => values.ToList();
}