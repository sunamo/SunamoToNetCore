namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoStringReplace;

internal class SHReplace
{
    /// <summary>
    /// Replace with index.
    /// </summary>
    internal static string ReplaceWithIndex(string text, string searchValue, string replacement, ref int foundIndex)
    {
        if (foundIndex == -1)
        {
            foundIndex = text.IndexOf(searchValue);
            if (foundIndex != -1)
            {
                text = text.Remove(foundIndex, searchValue.Length);
                text = text.Insert(foundIndex, replacement);
            }
        }

        return text;
    }

    /// <summary>
    /// Replace once.
    /// </summary>
    internal static string ReplaceOnce(string input, string pattern, string replacement)
    {
        return new Regex(pattern).Replace(input, replacement, 1);
    }
}