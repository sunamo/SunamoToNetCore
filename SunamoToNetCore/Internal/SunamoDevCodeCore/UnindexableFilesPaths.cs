namespace SunamoDevCodeCore;

internal class UnindexableFilesPaths
{
    public string FileUnindexableFileNames { get; set; }
    public string FileUnindexableFileNamesExactly { get; set; }
    public string FileUnindexablePathEnds { get; set; }
    public string FileUnindexablePathParts { get; set; }
    public string FileUnindexablePathStarts { get; set; }

    public UnindexableFilesPaths(string basePath)
    {
        var fileNames = UnindexableFilesNames.Instance;
        FileUnindexablePathParts = basePath + fileNames.FileUnindexablePathParts;
        FileUnindexableFileNames = basePath + fileNames.FileUnindexableFileNames;
        FileUnindexableFileNamesExactly = basePath + fileNames.FileUnindexableFileNamesExactly;
        FileUnindexablePathEnds = basePath + fileNames.FileUnindexablePathEnds;
        FileUnindexablePathStarts = basePath + fileNames.FileUnindexablePathStarts;
    }
}