namespace SunamoDevCodeCore.Interfaces;

internal interface ICsFileFilter
{
    List<string> GetFilesFiltered(string path, string searchPattern, SearchOption searchOption);
}