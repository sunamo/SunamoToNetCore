namespace SunamoToNetCore._sunamo.SunamoDevCodeBase;

internal class FSGetFiles
{

    /// <summary>
    /// Get files.
    /// </summary>
    internal static List<string> GetFiles(ILogger logger, string folder, string mask, bool isRecursive, GetFilesArgsDC? getFilesArgs = null)
    {
        return GetFiles(logger, folder, mask, isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly, getFilesArgs);
    }

#pragma warning disable
    /// <summary>
    /// Get files.
    /// </summary>
    internal static List<string> GetFiles(ILogger logger, string folder, string mask, SearchOption searchOption, GetFilesArgsDC? getFilesArgs = null)
#pragma warning restore
    {
        if (getFilesArgs != null)
        {
            ThrowEx.Custom("getFilesArgs is not null");
        }

        return Directory.GetFiles(folder, mask, searchOption).ToList();
    }

#pragma warning disable
    /// <summary>
    /// Get files.
    /// </summary>
    internal static List<string> GetFiles(ILogger logger, string folder, bool isRecursive)
#pragma warning restore
    {
        return Directory.GetFiles(folder, "*", isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
    }
}