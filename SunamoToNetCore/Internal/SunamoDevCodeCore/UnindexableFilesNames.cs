namespace SunamoDevCodeCore;

internal class UnindexableFilesNames
{
    public static UnindexableFilesNames Instance { get; } = new();
    public string FileUnindexableFileNames { get; } = "unindexableFileNames.txt";
    public string FileUnindexableFileNamesExactly { get; } = "unindexableFileNamesExactly.txt";
    public string FileUnindexablePathEnds { get; } = "unindexablePathEnds.txt";
    public string FileUnindexablePathParts { get; } = "unindexablePathParts.txt";
    public string FileUnindexablePathStarts { get; } = "unindexablePathStarts.txt";

    private UnindexableFilesNames()
    {
    }
}