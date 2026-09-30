namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class FS
{
    /// <summary>
    /// Delete folders which not contains.
    /// </summary>
    internal static void DeleteFoldersWhichNotContains(string rootPath, string folderPattern, IList<string> mustContainPatterns)
    {
        var folders = Directory.GetDirectories(rootPath, folderPattern, SearchOption.AllDirectories).ToList();
        for (int i = folders.Count - 1; i >= 0; i--)
        {
            if (CA.ReturnWhichContainsIndexes(folders[i], mustContainPatterns).Count != 0)
            {
                folders.RemoveAt(i);
            }
        }

        foreach (var folder in folders)
        {
        //FS.DeleteF
        }
    }

    /// <summary>
    /// Combine worker.
    /// </summary>
    private static string CombineWorker(bool isFirstCharUpper, bool isFile, params string[] pathParts)
    {
        for (var i = 0; i < pathParts.Length; i++)
            pathParts[i] = pathParts[i].TrimStart('\\');
        var result = Path.Combine(pathParts);
        if (result[2] != '\\')
            result = result.Insert(2, "\"");
        if (isFirstCharUpper)
            result = SH.FirstCharUpper(ref result);
        else
            result = SH.FirstCharUpper(ref result);
        if (!isFile)
            // Cant return with end slash becuase is working also with files
            WithEndSlash(ref result);
        return result;
    }

    /// <summary>
    /// Combine.
    /// </summary>
    internal static string Combine(params string[] pathParts)
    {
        return CombineWorker(true, false, pathParts);
    }

    /// <summary>
    /// Only names without extension copy.
    /// </summary>
    internal static List<string> OnlyNamesWithoutExtensionCopy(List<string> filePaths)
    {
        var result = new List<string>(filePaths.Count);
        for (var i = 0; i < filePaths.Count; i++)
            result.Add(Path.GetFileNameWithoutExtension(filePaths[i]));
        return result;
    }

    /// <summary>
    /// Replace directory throw exception if from doesnt exists.
    /// </summary>
    internal static string ReplaceDirectoryThrowExceptionIfFromDoesntExists(string path, string folderWithProjectsFolders, string folderWithTemporaryMovedContentWithoutBackslash)
    {
        path = SH.FirstCharUpper(path);
        folderWithProjectsFolders = SH.FirstCharUpper(folderWithProjectsFolders);
        folderWithTemporaryMovedContentWithoutBackslash = SH.FirstCharUpper(folderWithTemporaryMovedContentWithoutBackslash);
        if (!ThrowEx.NotContains(path, folderWithProjectsFolders))
            // Here can never accomplish when exc was throwed
            return path;
        // Here can never accomplish when exc was throwed
        return path.Replace(folderWithProjectsFolders, folderWithTemporaryMovedContentWithoutBackslash);
    }

    /// <summary>
    /// Make unc long path.
    /// </summary>
    internal static string MakeUncLongPath(ref string path)
    {
        if (!path.StartsWith(@"\\?\"))
        {
        // V ASP.net mi vrátilo u každé directory.exists false. Byl jsem pod ApplicationPoolIdentity v IIS a bylo nastaveno Full Control pro IIS AppPool\DefaultAppPool
        }

        return path;
    }

    /// <summary>
    /// Make unc long path.
    /// </summary>
    internal static string MakeUncLongPath(string path)
    {
        return MakeUncLongPath(ref path);
    }
}
