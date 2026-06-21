namespace SunamoDevCode.ToNetCore.research;

public partial class MoveToNet5
{
    /// <summary>
    /// Cached lines from the "don't replace references in" configuration file.
    /// </summary>
    public static List<string>? linesFromDontReplaceReferencesIn = null;

    /// <summary>
    /// Dont use XmlDocumentsCache 
    /// </summary>
    /// <returns></returns>
    public Tuple<List<string>, List<string>> WebAndNonWebProjects(ILogger logger, bool withCsprojs = true)
    {
        return ApsHelper.WebAndNonWebProjects(logger, withCsprojs);
    }

    /// <summary>
    /// Gets web and non-web solution paths separated into two lists.
    /// </summary>
    /// <returns>Tuple where Item1 is web solution paths and Item2 is non-web solution paths.</returns>
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