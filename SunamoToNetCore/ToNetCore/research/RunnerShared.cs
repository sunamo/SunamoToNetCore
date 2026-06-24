namespace SunamoToNetCore;

public partial class MoveToNet5
{
    public static List<string>? linesFromDontReplaceReferencesIn = null;

    // Dont use XmlDocumentsCache
    public Tuple<List<string>, List<string>> WebAndNonWebProjects(ILogger logger, bool withCsprojs = true)
    {
        return ApsHelper.WebAndNonWebProjects(logger, withCsprojs);
    }

    public Tuple<List<string>, List<string>> WebAndNonWebSlns()
    {
        List<string> webProjects = new();
        List<string> notWebProjects = new();

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