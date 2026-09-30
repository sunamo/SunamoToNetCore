namespace SunamoAps.Aps;

internal partial class ApsHelper : ApsPluginStatic
{
    public List<string> GetFilesForGitExcludeTemporary(ILogger logger, string folder, string typedExt)
    {
        var files = FSGetFiles.GetFiles(logger, folder, FS.MascFromExtension(typedExt), SearchOption.AllDirectories);
        SunamoDevCodeHelper.RemoveTemporaryFilesVS(files);
        return files;
    }

    public static List<string> FullPathsFromSolutionsNames(List<KeyValuePair<string, string>> slnNames)
    {
        var result = new List<string>();
        foreach (var item in slnNames)
        {
            var sln = SolutionsIndexerHelper.SolutionWithName(item.Value);
            result.Add(sln!.FullPathFolder);
        }

        return result;
    }
}