namespace SunamoDevCodeCore;

internal class UnindexableHelper
{
    public static Unindexable Unindexable = null!;

    public static PpkOnDriveDC UnindexablePathParts => Unindexable.UnindexablePathParts;

    public static PpkOnDriveDC UnindexableFileNames => Unindexable.UnindexableFileNames;

    public static PpkOnDriveDC UnindexablePathEnds => Unindexable.UnindexablePathEnds;

    public static PpkOnDriveDC UnindexablePathStarts => Unindexable.UnindexablePathStarts;

    //    Into A1 insert SearchCodeElementsUC .ufp
    public static void Load(UnindexableFilesPaths filePaths)
    {
        Unindexable = new Unindexable();

        //ClipboardService.SetText(filePaths.fileUnindexablePathParts);
        //PD.ShowMb(filePaths.fileUnindexablePathParts);

        Unindexable.UnindexablePathParts = new PpkOnDriveDC(filePaths.FileUnindexablePathParts);

        Unindexable.UnindexableFileNames = new PpkOnDriveDC(filePaths.FileUnindexableFileNames);
        Unindexable.UnindexableFileNamesExactly = new PpkOnDriveDC(filePaths.FileUnindexableFileNamesExactly);
        Unindexable.UnindexablePathEnds = new PpkOnDriveDC(filePaths.FileUnindexablePathEnds);
        Unindexable.UnindexablePathStarts = new PpkOnDriveDC(filePaths.FileUnindexablePathStarts);
    }

    public static bool IsToIndexedFolder(string path)
    {
        if (UnindexablePathStarts != null && UnindexablePathParts != null)
        {
            if (UnindexablePathParts.TrueForAll(part => !path.Contains(part)))
                if (UnindexablePathStarts.TrueForAll(start => !path.StartsWith(start)))
                    return true;
        }
        else
        {
            return true;
        }

        return false;
    }

    public static bool IsToIndexed(string path, string fileName, Func<string, bool> sci_IsIndexed)
    {
        if (UnindexablePathEnds != null && UnindexableFileNames != null)
        {
            //Checking for sth for which is checking in SourceCodeIndexerRoslyn.ProcessFile
            if (UnindexablePathEnds.TrueForAll(ending => !path.EndsWith(ending)))
                if (UnindexableFileNames.TrueForAll(name => !fileName.Contains(name)))
                {
                    if (sci_IsIndexed == null)
                        return true;
                    return sci_IsIndexed(path);
                }
        }
        else
        {
            return true;
        }

        return false;
    }
}