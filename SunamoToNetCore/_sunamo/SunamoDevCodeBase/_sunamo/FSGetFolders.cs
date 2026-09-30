namespace SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo;

internal class FSGetFolders
{

    /// <summary>
    /// Get folders.
    /// </summary>
    internal static IEnumerable<string> GetFolders(string path) => Directory.GetDirectories(path);
}