namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoStringParts;

internal class SHParts
{
    // The KeepAfterFirst method was removed - inlined in TypeScriptHelper.cs:79

    // The RemoveAfterLast method was removed - inlined in SolutionFolderSerialize.cs:57

    /// <summary>
    /// Remove after first.
    /// </summary>
    internal static string RemoveAfterFirst(string text, char searchChar)
    {
        int index = text.IndexOf(searchChar);
        return index == -1 || index == text.Length - 1 ? text : text.Substring(0, index);
    }

}