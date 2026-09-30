namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class FS
{

    /// <summary>
    /// Try delete directory.
    /// </summary>
    internal static bool TryDeleteDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            return true;
        }

        try
        {
            Directory.Delete(directoryPath, true);
            return true;
        }
        catch (Exception)
        {
        // EN: It's try so don't know what this is doing here
        // CZ: Je to try takže nevím co tu dělá tohle
        }

        var files = GetFiles(directoryPath, "*", SearchOption.AllDirectories);
        foreach (var filePath in files)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
        }

        try
        {
            Directory.Delete(directoryPath, true);
            return true;
        }
        catch (Exception)
        {
        }

        return false;
    }

    /// <summary>
    /// With end slash.
    /// </summary>
    internal static string WithEndSlash(string path)
    {
        return WithEndSlash(ref path);
    }

    /// <summary>
    /// With end slash.
    /// </summary>
    internal static string WithEndSlash(ref string path)
    {
        if (path != string.Empty)
        {
            path = path.TrimEnd('\\') + '\\';
        }

        SH.FirstCharUpper(ref path);
        return path;
    }

    /// <summary>
    /// Insert between file name and extension.
    /// </summary>
    internal static string InsertBetweenFileNameAndExtension(string originalPath, string textToInsert)
    {
        //return InsertBetweenFileNameAndExtension<string, string>(originalPath, textToInsert, null);
        // Cesta by se zde hodila kvůli FS.CiStorageFile
        // nicméně StorageFolder nevím zda se používá, takže to bude umět i bez toho
        var originalPathString = originalPath.ToString();
        string fileName = Path.GetFileNameWithoutExtension(originalPathString);
        string extension = Path.GetExtension(originalPathString);
        if (originalPathString.Contains('/') || originalPathString.Contains('\\'))
        {
            string directory = Path.GetDirectoryName(originalPathString)!;
            return Path.Combine(directory, fileName + textToInsert + extension);
        }

        return fileName + textToInsert + extension;
    }

    /// <summary>
    /// Only names no direct edit.
    /// </summary>
    internal static List<string> OnlyNamesNoDirectEdit(String[] filePaths)
    {
        var list = filePaths.ToList();
        return OnlyNamesNoDirectEdit(list);
    }

    // No direct edit
    // Returns with extension
    // POZOR: Na rozdíl od stejné metody v sunamo tato metoda vrací úplně nové pole a nemodifikuje A1
    /// <summary>
    /// Only names no direct edit.
    /// </summary>
    internal static List<string> OnlyNamesNoDirectEdit(List<string> filePaths)
    {
        var fileNames = new List<string>(filePaths.Count);
        for (int i = 0; i < filePaths.Count; i++)
        {
            fileNames.Add(Path.GetFileName(filePaths[i]));
        }

        return fileNames;
    }

    /// <summary>
    /// Get files.
    /// </summary>
    internal static List<string> GetFiles(string folder, string mask, SearchOption searchOption /*, GetFilesArgsDC getFilesArgs = null*/)
    {
        //ThrowEx.NotImplementedMethod();
        return Directory.GetFiles(folder, mask, searchOption).ToList();
    }

    /// <summary>
    /// Get directory name.
    /// </summary>
    internal static string GetDirectoryName(string filePath)
    {
        var data = Path.GetDirectoryName(filePath)!;
        return FS.WithEndSlash(data);
    }

    /// <summary>
    /// Get file name.
    /// </summary>
    internal static string GetFileName(string fullPathFolder)
    {
        return FS.GetFileName(fullPathFolder);
    }

    /// <summary>
    /// Exists file.
    /// </summary>
    internal static bool ExistsFile(string filePath)
    {
        return File.Exists(filePath);
    }

    /// <summary>
    /// Exists directory.
    /// </summary>
    internal static bool ExistsDirectory(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }
}