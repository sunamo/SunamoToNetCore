namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

internal class CAG
{
    // Return what exists in both lists
    // Modify both firstList and secondList - keep only which is only in one
    /// <summary>
    /// Compare list.
    /// </summary>
    internal static List<T> CompareList<T>(List<T> firstList, List<T> secondList) where T : IEquatable<T>
    {
        var existsInBoth = new List<T>();

        int foundIndex;

        for (int i = secondList.Count - 1; i >= 0; i--)
        {
            T currentItem = secondList[i];
            foundIndex = firstList.IndexOf(currentItem);

            if (foundIndex != -1)
            {
                existsInBoth.Add(currentItem);
                secondList.RemoveAt(i);
                firstList.RemoveAt(foundIndex);
            }
        }

        for (int i = firstList.Count - 1; i >= 0; i--)
        {
            T currentItem = firstList[i];
            foundIndex = secondList.IndexOf(currentItem);

            if (foundIndex != -1)
            {
                existsInBoth.Add(currentItem);
                firstList.RemoveAt(i);
                secondList.RemoveAt(foundIndex);
            }
        }

        return existsInBoth;
    }
}
