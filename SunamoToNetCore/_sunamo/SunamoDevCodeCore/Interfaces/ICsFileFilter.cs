namespace SunamoToNetCore._sunamo.SunamoDevCodeCore;

internal interface ICsFileFilter
{
    /// <summary>
    /// Get files filtered.
    /// </summary>
    List<string> GetFilesFiltered(string path, string searchPattern, SearchOption searchOption);
}