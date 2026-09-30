namespace SunamoAps.Aps.Projs;

internal partial class VsProjectsFileHelper
{
    static Type type = typeof(VsProjectsFileHelper);
    #region Bez používání externích knihoven
    /// <summary>
    /// Return paths of all csprojs
    /// </summary>
    /// <param name="logger">Logger instance for logging operations.</param>
    /// <param name="path">Root path to search for csproj files.</param>
    /// <returns>List of all csproj file paths found.</returns>
    public static List<string> DuplicatedCsproj(ILogger logger, string path)
    {
        var result = FSGetFiles.GetFiles(logger, path, "*.csproj", true);
        var allCsproj = result.ToList();
        CAChangeContent.ChangeContent0(null!, allCsproj, FS.GetFileName /*SHParts.RemoveAfterLast, "\\"*/);
        allCsproj.RemoveAll(d => d == "Runner.csproj");
        var dupl = CAG.GetDuplicities(allCsproj);
        if (dupl.Count > 0)
        {
            ThrowEx.Custom("Some csprojs have duplicated file names: " + SHJoin.JoinNL(dupl) + ". List was also copied to clipboard.");
        }
        return result;
    }
    #endregion
}