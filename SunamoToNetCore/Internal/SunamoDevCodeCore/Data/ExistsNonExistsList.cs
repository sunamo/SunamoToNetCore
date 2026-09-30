namespace SunamoDevCodeCore.Data;

internal class ExistsNonExistsList<T>
{
    public List<T> Exists { get; set; } = new List<T>();

    public List<T> NonExists { get; set; } = new List<T>();
}