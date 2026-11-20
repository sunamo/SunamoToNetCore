namespace SunamoToNetCore.research;

public partial class MoveToNet5
{
    /// <summary>
    /// The lines from dont replace references in.
    /// </summary>
    public static List<string>? linesFromDontReplaceReferencesIn = null;

    // Dont use XmlDocumentsCache
    /// <summary>
    /// Web and non web projects.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="withCsprojs">The with csprojs.</param>
    /// <returns>The web and non web projects result.</returns>
    public Tuple<List<string>, List<string>> WebAndNonWebProjects(ILogger logger, bool withCsprojs = true)
    {
        return ApsHelper.WebAndNonWebProjects(logger, withCsprojs);
    }

    /// <summary>
    /// Web and non web slns.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="withCsprojs">The with csprojs.</param>
    /// <returns>The web and non web slns result.</returns>
    public Tuple<List<string>, List<string>> WebAndNonWebSlns()
    {
        List<string> webProjects = new List<string>();
        List<string> notWebProjects = new List<string>();

        foreach (var item in FoldersWithSolutions.Fwss)
        {
            var text = item.GetSolutions(RepositoryLocal.Vs17);
            foreach (var sln in text)
            {
                var slnFullPathFolder = sln.FullPathFolder;
                if (ApsHelper.IsWeb(slnFullPathFolder))
                {
                    webProjects.Add(slnFullPathFolder);
                }
                else
                {
                    notWebProjects.Add(slnFullPathFolder);
                }
            }
        }

        return new Tuple<List<string>, List<string>>(webProjects, notWebProjects);
    }


}