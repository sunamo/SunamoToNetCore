namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoCollectionsChangeContent;

internal class CAChangeContent
{
    /// <summary>
    /// Remove null or empty.
    /// </summary>
    private static void RemoveNullOrEmpty(ChangeContentArgsDC args, List<string> list)
    {
        if (args != null)
        {
            if (args.RemoveNull)
            {
                list.Remove(null!);
            }
            if (args.RemoveEmpty)
            {
                for (int index = list.Count - 1; index >= 0; index--)
                {
                    if (list[index].Trim() == string.Empty)
                    {
                        list.RemoveAt(index);
                    }
                }
            }
        }
    }

    // Direct edit - Changes content of list using provided function with 0 additional parameters
    // If not every element fulfills pattern, it is good to remove null (or values returned if can't be changed) from result
    /// <summary>
    /// Change content0.
    /// </summary>
    internal static List<string> ChangeContent0(ChangeContentArgsDC args, List<string> list, Func<string, string> func)
    {
        for (int index = 0; index < list.Count; index++)
        {
            list[index] = func.Invoke(list[index]);
        }
        RemoveNullOrEmpty(args, list);
        return list;
    }
}